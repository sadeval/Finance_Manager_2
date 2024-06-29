using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace FinancialApp
{
    public class FinancialManager
    {
        private List<Transaction> transactions = new List<Transaction>();
        private int nextId = 1;
        private readonly string filePath = @"D:\transactions.txt";
        private string currentCurrency = "UAH";

        public FinancialManager()
        {
            LoadTransactions();
        }

        public void AddTransaction()
        {
            Console.WriteLine("\nВыберите тип транзакции: \n");
            Console.WriteLine("1. Доход");
            Console.WriteLine("2. Расход\n");
            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddIncome();
                    break;
                case "2":
                    AddExpense();
                    break;
                default:
                    Console.WriteLine("Неверный выбор. Попробуйте еще раз.");
                    break;
            }
        }

        private void AddIncome()
        {
            Console.WriteLine("\nВыберите тип дохода: \n");
            Console.WriteLine("1. Зарплата");
            Console.WriteLine("2. Аренда");
            Console.WriteLine("3. Подарок");
            Console.WriteLine("4. Выигрыш\n");
            string incomeType = Console.ReadLine() switch
            {
                "1" => "Зарплата",
                "2" => "Аренда",
                "3" => "Подарок",
                "4" => "Выигрыш",
                _ => "Другой"
            };

            decimal amount = GetAmount();
            DateTime date = GetDate();
            Console.Write("\nВведите описание: ");
            string? description = Console.ReadLine();

            Income income = new Income(nextId++, amount, date, description, currentCurrency, incomeType);
            transactions.Add(income);
            SaveTransactions();
        }

        private void AddExpense()
        {
            Console.WriteLine("Выберите тип расхода: \n");
            Console.WriteLine("1. Квартплата");
            Console.WriteLine("2. Коммунальные услуги");
            Console.WriteLine("3. Машина");
            Console.WriteLine("4. Телефон");
            Console.WriteLine("5. Еда");
            Console.WriteLine("6. Пожертвование");
            Console.WriteLine("7. Покупки");
            Console.WriteLine("8. Путешествия");
            Console.WriteLine("9. Кредит\n");
            string expenseType = Console.ReadLine() switch
            {
                "1" => "Квартплата",
                "2" => "Коммунальные услуги",
                "3" => "Машина",
                "4" => "Телефон",
                "5" => "Еда",
                "6" => "Пожертвование",
                "7" => "Покупки",
                "8" => "Путешествия",
                "9" => "Кредит",
                _ => "Другой"
            };

            decimal amount = GetAmount();
            DateTime date = GetDate();
            Console.Write("\nВведите описание: ");
            string? description = Console.ReadLine();

            Expense expense = new Expense(nextId++, amount, date, description, currentCurrency, expenseType);
            transactions.Add(expense);
            SaveTransactions();
        }

        private decimal GetAmount()
        {
            decimal amount;
            while (true)
            {
                Console.Write("\nВведите сумму: ");
                try
                {
                    amount = Convert.ToDecimal(Console.ReadLine(), CultureInfo.InvariantCulture);
                    break;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Недопустимая сумма. Пожалуйста, введите допустимое десятичное число.");
                }
            }
            return amount;
        }

        private DateTime GetDate()
        {
            DateTime date;
            while (true)
            {
                Console.Write("Введите дату (MM.DD.YYYY): ");
                try
                {
                    date = DateTime.ParseExact(Console.ReadLine(), "MM.dd.yyyy", CultureInfo.InvariantCulture);
                    break;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Неверный формат даты. Введите дату в формате MM.DD.YYYY.");
                }
            }
            return date;
        }

        public void DeleteTransaction(int id)
        {
            Transaction? transactionToRemove = transactions.Find(t => t.Id == id);
            if (transactionToRemove != null)
            {
                transactions.Remove(transactionToRemove);
                SaveTransactions();
                Console.WriteLine($"Транзакция с ID {id} удалена.");
            }
            else
            {
                Console.WriteLine("Транзакция не найдена.");
            }
        }

        public void DisplayTransactions()
        {
            foreach (var transaction in transactions)
            {
                Console.WriteLine(transaction);
            }
        }

        public decimal GetBalance()
        {
            decimal balance = 0;
            foreach (var transaction in transactions)
            {
                if (transaction is Income)
                {
                    balance += transaction.Amount;
                }
                else if (transaction is Expense)
                {
                    balance -= transaction.Amount;
                }
            }
            return balance;
        }

        public void SetCurrency(string currency)
        {
            currentCurrency = currency;
        }

        public string GetCurrency()
        {
            return currentCurrency;
        }

        private void SaveTransactions()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    foreach (var transaction in transactions)
                    {
                        string line = $"{transaction.Id},{transaction.Amount},{transaction.Date:MM.dd.yyyy},{transaction.Description},{transaction.Currency},{(transaction is Income ? ((Income)transaction).IncomeType : ((Expense)transaction).ExpenseType)}";
                        writer.WriteLine(line);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка при сохранении транзакции: {ex.Message}");
            }
        }

        private void LoadTransactions()
        {
            try
            {
                if (File.Exists(filePath))
                {
                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        string? line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            string[] parts = line.Split(',');

                            if (parts.Length == 6)
                            {
                                int id = int.Parse(parts[0]);
                                decimal amount = decimal.Parse(parts[1], CultureInfo.InvariantCulture);
                                DateTime date = DateTime.ParseExact(parts[2], "MM.dd.yyyy", CultureInfo.InvariantCulture);
                                string description = parts[3];
                                string currency = parts[4];
                                string type = parts[5];

                                Transaction transaction = type switch
                                {
                                    "Зарплата" or "Аванс" or "Подарок" or "Выигрыш" => new Income(id, amount, date, description, currency, type),
                                    "Квартплата" or "Коммунальные услуги" or "Такси" or "Телефон" or "Еда" or "Пожертвование" or "Покупки" or "Путешествия" or "Кредит" => new Expense(id, amount, date, description, currency, type),
                                    _ => throw new Exception("Неизвестный тип транзакции.")
                                };

                                transactions.Add(transaction);
                            }
                        }

                        if (transactions.Count > 0)
                        {
                            nextId = transactions[^1].Id + 1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка при загрузке транзакции: {ex.Message}");
            }
        }
    }
}
