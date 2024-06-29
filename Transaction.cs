using System;
using System.Globalization;

namespace FinancialApp
{
    public class Transaction
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string? Description { get; set; }
        public string? Currency { get; set; }
        public virtual string? TransactionType { get; }

        public Transaction(int id, decimal amount, DateTime date, string description, string currency)
        {
            Id = id;
            Amount = amount;
            Date = date;
            Description = description;
            Currency = currency;
        }

        public override string ToString()
        {
            string idColumn = $"|  {Id}  ".PadRight(5);
            string amountColumn = $"|  {Amount} {Currency} ".PadRight(15);
            string dateColumn = $"|  {Date.ToShortDateString()}".PadRight(15);
            string descriptionColumn = $"|  {Description}   ".PadRight(20);
            string transactionTypeColumn = $"|  {TransactionType}".PadRight(23);

            return $"\n======================================================================================\n" +
                   $"|  ID: |     Сумма:    |     Дата:     |  Описание:         |  Тип транзакции:       |\n" +
                   $"-------|---------------|---------------|--------------------|------------------------|\n" +
                   $"{idColumn} {amountColumn} {dateColumn} {descriptionColumn} {transactionTypeColumn}  |\n" +
                   $"======================================================================================";
        }
    }

    public class Income : Transaction
    {
        public string IncomeType { get; set; }

        public override string TransactionType => IncomeType;

        public Income(int id, decimal amount, DateTime date, string description, string currency, string incomeType)
            : base(id, amount, date, description, currency)
        {
            IncomeType = incomeType;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }

    public class Expense : Transaction
    {
        public string ExpenseType { get; set; }

        public override string TransactionType => ExpenseType;

        public Expense(int id, decimal amount, DateTime date, string description, string currency, string expenseType)
            : base(id, amount, date, description, currency)
        {
            ExpenseType = expenseType;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
