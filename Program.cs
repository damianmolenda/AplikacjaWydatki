using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplikacjaWydatki
{
    class Expense
    {
        public string Category { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }

    class ExpenseManager
    {
        private List<Expense> expenseList;

        public ExpenseManager()
        {
            expenseList = new List<Expense>();
        }

        public void AddExpense(string category, decimal amount, DateTime date)
        {
            Expense newExpense = new Expense
            {
                Category = category,
                Amount = amount,
                Date = date
            };
            expenseList.Add(newExpense);
            Console.WriteLine("Dodano nowy wydatek.");
        }

        public List<Expense> GetExpensesInPeriod(DateTime start, DateTime end)
        {
            return expenseList.Where(e => e.Date >= start && e.Date <= end).ToList();
        }

        public Dictionary<string, decimal> GenerateCategoryReport(DateTime start, DateTime end)
        {
            var expensesInPeriod = GetExpensesInPeriod(start, end);
            var report = new Dictionary<string, decimal>();

            foreach (var expense in expensesInPeriod)
            {
                if (report.ContainsKey(expense.Category))
                    report[expense.Category] += expense.Amount;
                else
                    report[expense.Category] = expense.Amount;
            }

            return report;
        }

        public decimal CalculateTotalExpenses(DateTime start, DateTime end)
        {
            var expensesInPeriod = GetExpensesInPeriod(start, end);
            return expensesInPeriod.Sum(e => e.Amount);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            ExpenseManager expenseManager = new ExpenseManager();

            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("1. Dodaj nowy wydatek");
                Console.WriteLine("2. Generuj raport z wydatków w określonym okresie");
                Console.WriteLine("3. Oblicz całkowitą kwotę wydatków w określonym okresie");
                Console.WriteLine("4. Zakończ");

                Console.Write("Wybierz opcję: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("Podaj kategorię wydatku: ");
                        string category = Console.ReadLine();

                        Console.Write("Podaj kwotę wydatku: ");
                        decimal amount;
                        while (!decimal.TryParse(Console.ReadLine(), out amount))
                        {
                            Console.Write("Nieprawidłowa kwota. Podaj ponownie: ");
                        }

                        Console.Write("Podaj datę wydatku (RRRR-MM-DD): ");
                        DateTime date;
                        while (!DateTime.TryParse(Console.ReadLine(), out date))
                        {
                            Console.Write("Nieprawidłowa data. Podaj ponownie (RRRR-MM-DD): ");
                        }

                        expenseManager.AddExpense(category, amount, date);
                        break;

                    case "2":
                        Console.Write("Podaj początkową datę okresu (RRRR-MM-DD): ");
                        DateTime startDate;
                        while (!DateTime.TryParse(Console.ReadLine(), out startDate))
                        {
                            Console.Write("Nieprawidłowa data. Podaj ponownie (RRRR-MM-DD): ");
                        }

                        Console.Write("Podaj końcową datę okresu (RRRR-MM-DD): ");
                        DateTime endDate;
                        while (!DateTime.TryParse(Console.ReadLine(), out endDate))
                        {
                            Console.Write("Nieprawidłowa data. Podaj ponownie (RRRR-MM-DD): ");
                        }

                        var report = expenseManager.GenerateCategoryReport(startDate, endDate);

                        Console.WriteLine("Raport z wydatków:");
                        foreach (var kvp in report)
                        {
                            Console.WriteLine($"{kvp.Key}: {kvp.Value}");
                        }
                        break;

                    case "3":
                        Console.Write("Podaj początkową datę okresu (RRRR-MM-DD): ");
                        DateTime startSumDate;
                        while (!DateTime.TryParse(Console.ReadLine(), out startSumDate))
                        {
                            Console.Write("Nieprawidłowa data. Podaj ponownie (RRRR-MM-DD): ");
                        }

                        Console.Write("Podaj końcową datę okresu (RRRR-MM-DD): ");
                        DateTime endSumDate;
                        while (!DateTime.TryParse(Console.ReadLine(), out endSumDate))
                        {
                            Console.Write("Nieprawidłowa data. Podaj ponownie (RRRR-MM-DD): ");
                        }

                        decimal totalExpenses = expenseManager.CalculateTotalExpenses(startSumDate, endSumDate);
                        Console.WriteLine($"Sumaryczna kwota wydatków: {totalExpenses}");
                        break;

                    case "4":
                        exit = true;
                        break;

                    default:
                        Console.WriteLine("Nieprawidłowy wybór.");
                        break;
                }

                Console.WriteLine();
            }
        }
    }
}

