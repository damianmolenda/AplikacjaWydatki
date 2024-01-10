using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplikacjaWydatki
{
    interface IManageCategories
    {
        void AddCategory(string category);
        void RemoveCategory(string category);
        void EditCategory(string oldCategory, string newCategory);
        void ShowAllCategories();
    }

    class CategoryManager : IManageCategories
    {
        private List<string> categories;

        public CategoryManager()
        {
            categories = new List<string>();
        }

        public void AddCategory(string category)
        {
            categories.Add(category);
            Console.WriteLine($"Dodano kategorię: {category}");
        }

        public void RemoveCategory(string category)
        {
            if (categories.Contains(category))
            {
                categories.Remove(category);
                Console.WriteLine($"Usunięto kategorię: {category}");
            }
            else
            {
                Console.WriteLine("Podana kategoria nie istnieje.");
            }
        }

        public void EditCategory(string oldCategory, string newCategory)
        {
            if (categories.Contains(oldCategory))
            {
                int index = categories.IndexOf(oldCategory);
                categories[index] = newCategory;
                Console.WriteLine($"Zmieniono nazwę kategorii z {oldCategory} na {newCategory}");
            }
            else
            {
                Console.WriteLine("Podana kategoria nie istnieje.");
            }
        }

        public virtual void ShowAllCategories()
        {
            Console.WriteLine("Lista wszystkich kategorii:");
            foreach (var category in categories)
            {
                Console.WriteLine(category);
            }
        }
    }

    class Expense
    {
        public string Category { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }

    class ExpenseManager
    {
        private List<Expense> expenseList;
        CategoryManager categoryManager;
        public ExpenseManager(CategoryManager categoryManager)
        {
            this.categoryManager = categoryManager;
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
            CategoryManager categoryManager;
            ExpenseManager expenseManager;

            categoryManager = new CategoryManager();
            expenseManager = new ExpenseManager(categoryManager);
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("1. Dodaj nowy wydatek");
                Console.WriteLine("2. Generuj raport z wydatków w określonym okresie");
                Console.WriteLine("3. Oblicz całkowitą kwotę wydatków w określonym okresie");
                Console.WriteLine("4. Zarządzaj kategoriami");
                Console.WriteLine("5. Zakończ");

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
                        bool categoryMenu = true;

                        while (categoryMenu)
                        {
                            Console.WriteLine("1. Dodaj kategorię");
                            Console.WriteLine("2. Usuń kategorię");
                            Console.WriteLine("3. Zmień nazwę kategorii");
                            Console.WriteLine("4. Pokaż wszystkie kategorie");
                            Console.WriteLine("5. Powrót do głównego menu");

                            Console.Write("Wybierz opcję: ");
                            string categoryChoice = Console.ReadLine();

                            switch (categoryChoice)
                            {
                                case "1":
                                    Console.Write("Podaj nazwę kategorii: ");
                                    string newCategory = Console.ReadLine();
                                    categoryManager.AddCategory(newCategory);
                                    break;

                                case "2":
                                    Console.Write("Podaj nazwę kategorii do usunięcia: ");
                                    string categoryToRemove = Console.ReadLine();
                                    categoryManager.RemoveCategory(categoryToRemove);
                                    break;

                                case "3":
                                    Console.Write("Podaj nazwę istniejącej kategorii do zmiany: ");
                                    string oldCategory = Console.ReadLine();
                                    Console.Write("Podaj nową nazwę kategorii: ");
                                    string updatedCategory = Console.ReadLine();
                                    categoryManager.EditCategory(oldCategory, updatedCategory);
                                    break;

                                case "4":
                                    categoryManager.ShowAllCategories();
                                    break;

                                case "5":
                                    categoryMenu = false;
                                    break;

                                default:
                                    Console.WriteLine("Nieprawidłowa opcja.");
                                    break;
                            }
                        }
                        break;

                    case "5":
                        exit = true;
                        break;
                }
            }

        }
    }
}