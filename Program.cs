using System;

namespace FinancialApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "Менеджер финансов";

            FinancialManager manager = new FinancialManager();

            while (true)
            {
                Console.WriteLine("\nМеню:\n");
                Console.WriteLine("1. Добавить транзакцию");
                Console.WriteLine("2. Показать транзакции");
                Console.WriteLine("3. Показать баланс");
                Console.WriteLine("4. Установить валюту");
                Console.WriteLine("5. Удалить транзакцию");
                Console.WriteLine("6. Выход\n");
                Console.Write("Выберите действие: ");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        manager.AddTransaction();
                        break;

                    case "2":
                        manager.DisplayTransactions();
                        break;

                    case "3":
                        Console.Write("\n====================================\n");
                        Console.WriteLine($"| Текущий баланс: {manager.GetBalance()} {manager.GetCurrency()}        |");
                        Console.Write("====================================\n");
                        break;

                    case "4":
                        Console.Write("Введите новую валюту (USD, EUR, UAH): ");
                        string newCurrency = Console.ReadLine().ToUpper();
                        manager.SetCurrency(newCurrency);
                        Console.WriteLine($"Установлена валюта {manager.GetCurrency()}");
                        break;

                    case "5":
                        int idToDelete;
                        while (true)
                        {
                            Console.Write("Выберите ID транзакции для удаления: ");
                            try
                            {
                                idToDelete = Convert.ToInt32(Console.ReadLine());
                                break;
                            }
                            catch (FormatException)
                            {
                                Console.WriteLine("Неверное ID. Пожалуйста, введите допустимое целое число ID.");
                            }
                            catch (OverflowException)
                            {
                                Console.WriteLine("Число слишком велико. Пожалуйста, введите допустимое целое число ID.");
                            }
                        }
                        manager.DeleteTransaction(idToDelete);
                        break;

                    case "6":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте еще раз.");
                        break;
                }
            }
        }
    }
}
