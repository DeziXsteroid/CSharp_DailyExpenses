using System.Globalization;
using System.Text;

namespace CSharp_DailyExpenses;

internal class Program
{
    private static readonly CultureInfo RussianCulture = CultureInfo.GetCultureInfo("ru-RU");

    private static void Main()
    {
        Console.InputEncoding = Encoding.UTF8;
        Console.OutputEncoding = Encoding.UTF8;

        Console.WriteLine("=== Учёт трат за день ===");
        int operationCount = ReadInteger("Введите количество операций (2–40): ", 2, 40);
        Expense[] expenses = ReadExpenses(operationCount);

        RunMenu(expenses);
    }

    private static Expense[] ReadExpenses(int count)
    {
        Expense[] expenses = new Expense[count];

        Console.WriteLine();
        Console.WriteLine("Вводите траты по шаблону: Название; Сумма");
        Console.WriteLine("Пример: Влажные салфетки \"Лента\"; 235");

        for (int i = 0; i < count; i++)
        {
            while (true)
            {
                Console.Write($"{i + 1}. ");
                string input = Console.ReadLine() ?? string.Empty;
                int separatorIndex = input.LastIndexOf(';');

                if (separatorIndex <= 0 || separatorIndex == input.Length - 1)
                {
                    Console.WriteLine("Ошибка: укажите название и сумму через точку с запятой.");
                    continue;
                }

                string name = input[..separatorIndex].Trim();
                string amountText = input[(separatorIndex + 1)..].Trim();

                if (name.Length == 0)
                {
                    Console.WriteLine("Ошибка: название не может быть пустым.");
                    continue;
                }

                if (!TryParseDecimal(amountText, out decimal amount) || amount <= 0)
                {
                    Console.WriteLine("Ошибка: сумма должна быть положительным числом.");
                    continue;
                }

                expenses[i] = new Expense(name, amount);
                break;
            }
        }

        return expenses;
    }

    private static void RunMenu(Expense[] expenses)
    {
        while (true)
        {
            PrintMenu();
            int menuItem = ReadInteger("Выберите пункт: ", 0, 5);
            Console.WriteLine();

            switch (menuItem)
            {
                case 1:
                    PrintExpenses(expenses, "Все траты");
                    break;
                case 2:
                    PrintStatistics(expenses);
                    break;
                case 3:
                    Expense[] sortedExpenses = BubbleSortByAmount(expenses);
                    PrintExpenses(sortedExpenses, "Траты по возрастанию цены");
                    break;
                case 4:
                    ConvertCurrency(expenses);
                    break;
                case 5:
                    SearchByName(expenses);
                    break;
                case 0:
                    Console.WriteLine("Работа программы завершена.");
                    return;
            }
        }
    }

    private static void PrintMenu()
    {
        Console.WriteLine();
        Console.WriteLine("=== Меню ===");
        Console.WriteLine("1. Вывод данных");
        Console.WriteLine("2. Статистика (среднее, максимальное, минимальное, сумма)");
        Console.WriteLine("3. Сортировка по цене (пузырьковая сортировка)");
        Console.WriteLine("4. Конвертация валюты");
        Console.WriteLine("5. Поиск по названию");
        Console.WriteLine("0. Выход");
    }

    private static void PrintExpenses(Expense[] expenses, string title)
    {
        Console.WriteLine($"--- {title} ---");
        for (int i = 0; i < expenses.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {expenses[i].Name} — {FormatMoney(expenses[i].Amount)} руб.");
        }
    }

    private static void PrintStatistics(Expense[] expenses)
    {
        decimal sum = 0;
        decimal minimum = expenses[0].Amount;
        decimal maximum = expenses[0].Amount;

        foreach (Expense expense in expenses)
        {
            sum += expense.Amount;

            if (expense.Amount < minimum)
            {
                minimum = expense.Amount;
            }

            if (expense.Amount > maximum)
            {
                maximum = expense.Amount;
            }
        }

        decimal average = sum / expenses.Length;

        Console.WriteLine("--- Статистика ---");
        Console.WriteLine($"Сумма: {FormatMoney(sum)} руб.");
        Console.WriteLine($"Среднее: {FormatMoney(average)} руб.");
        Console.WriteLine($"Минимальное: {FormatMoney(minimum)} руб.");
        Console.WriteLine($"Максимальное: {FormatMoney(maximum)} руб.");
    }

    private static Expense[] BubbleSortByAmount(Expense[] source)
    {
        Expense[] result = (Expense[])source.Clone();

        for (int i = 0; i < result.Length - 1; i++)
        {
            bool wasSwapped = false;

            for (int j = 0; j < result.Length - i - 1; j++)
            {
                if (result[j].Amount > result[j + 1].Amount)
                {
                    (result[j], result[j + 1]) = (result[j + 1], result[j]);
                    wasSwapped = true;
                }
            }

            if (!wasSwapped)
            {
                break;
            }
        }

        return result;
    }

    private static void ConvertCurrency(Expense[] expenses)
    {
        Console.Write("Введите название валюты (например, USD): ");
        string currency = (Console.ReadLine() ?? string.Empty).Trim().ToUpperInvariant();

        if (currency.Length == 0)
        {
            currency = "ВАЛЮТА";
        }

        decimal rate = ReadPositiveDecimal($"Введите курс: сколько рублей стоит 1 {currency}: ");

        Console.WriteLine($"--- Траты в {currency} ---");
        foreach (Expense expense in expenses)
        {
            decimal convertedAmount = expense.Amount / rate;
            Console.WriteLine($"{expense.Name} — {convertedAmount.ToString("0.00", RussianCulture)} {currency}");
        }
    }

    private static void SearchByName(Expense[] expenses)
    {
        Console.Write("Введите название или его часть: ");
        string searchText = (Console.ReadLine() ?? string.Empty).Trim();
        bool found = false;

        Console.WriteLine("--- Результат поиска ---");
        foreach (Expense expense in expenses)
        {
            if (expense.Name.Contains(searchText, StringComparison.CurrentCultureIgnoreCase))
            {
                Console.WriteLine($"{expense.Name} — {FormatMoney(expense.Amount)} руб.");
                found = true;
            }
        }

        if (!found)
        {
            Console.WriteLine("Совпадений не найдено.");
        }
    }

    private static int ReadInteger(string prompt, int minimum, int maximum)
    {
        while (true)
        {
            Console.Write(prompt);
            string input = Console.ReadLine() ?? string.Empty;

            if (int.TryParse(input, out int value) && value >= minimum && value <= maximum)
            {
                return value;
            }

            Console.WriteLine($"Ошибка: введите целое число от {minimum} до {maximum}.");
        }
    }

    private static decimal ReadPositiveDecimal(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string input = Console.ReadLine() ?? string.Empty;

            if (TryParseDecimal(input, out decimal value) && value > 0)
            {
                return value;
            }

            Console.WriteLine("Ошибка: введите положительное число.");
        }
    }

    private static bool TryParseDecimal(string text, out decimal value)
    {
        string normalizedText = text.Trim().Replace(',', '.');
        return decimal.TryParse(
            normalizedText,
            NumberStyles.AllowDecimalPoint,
            CultureInfo.InvariantCulture,
            out value);
    }

    private static string FormatMoney(decimal amount)
    {
        return amount.ToString("0.00", RussianCulture);
    }
}

internal class Expense
{
    public Expense(string name, decimal amount)
    {
        Name = name;
        Amount = amount;
    }

    public string Name { get; }

    public decimal Amount { get; }
}
