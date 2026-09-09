Console.WriteLine("Учет трат за день");

int count;

while (true)
{
    Console.Write("Введите количество трат от 2 до 40: ");

    if (int.TryParse(Console.ReadLine(), out count) && count >= 2 && count <= 40)
    {
        break;
    }

    Console.WriteLine("Неверное число");
}

string[] names = new string[count];
double[] prices = new double[count];

for (int i = 0; i < count; i++)
{
    while (true)
    {
        Console.Write($"Введите трату {i + 1} (название; сумма): ");
        string text = Console.ReadLine() ?? "";
        string[] parts = text.Split(';');

        if (parts.Length == 2 &&
            parts[0].Trim() != "" &&
            double.TryParse(parts[1].Trim().Replace('.', ','), out double price) &&
            price > 0)
        {
            names[i] = parts[0].Trim();
            prices[i] = price;
            break;
        }

        Console.WriteLine("Неверный ввод. Пример: Хлеб; 80");
    }
}

int choice = -1;

while (choice != 0)
{
    Console.WriteLine();
    Console.WriteLine("1. Вывод данных");
    Console.WriteLine("2. Статистика");
    Console.WriteLine("3. Сортировка по цене");
    Console.WriteLine("4. Конвертация валюты");
    Console.WriteLine("5. Поиск по названию");
    Console.WriteLine("0. Выход");
    Console.Write("Ваш выбор: ");

    if (!int.TryParse(Console.ReadLine(), out choice))
    {
        Console.WriteLine("Неверный пунк");
        continue;
    }

    switch (choice)
    {
        case 1:
            for (int i = 0; i < count; i++)
            {
                Console.WriteLine($"{names[i]} - {prices[i]:F2} руб.");
            }
            break;

        case 2:
            double sum = 0;
            double min = prices[0];
            double max = prices[0];

            for (int i = 0; i < count; i++)
            {
                sum += prices[i];

                if (prices[i] < min)
                {
                    min = prices[i];
                }

                if (prices[i] > max)
                {
                    max = prices[i];
                }
            }

            Console.WriteLine($"Сумма: {sum:F2} руб.");
            Console.WriteLine($"Среднее: {sum / count:F2} руб.");
            Console.WriteLine($"Минимальное: {min:F2} руб.");
            Console.WriteLine($"Максимальное: {max:F2} руб.");
            break;

        case 3:
            for (int i = 0; i < count - 1; i++)
            {
                for (int j = 0; j < count - i - 1; j++)
                {
                    if (prices[j] > prices[j + 1])
                    {
                        double tempPrice = prices[j];
                        prices[j] = prices[j + 1];
                        prices[j + 1] = tempPrice;

                        string tempName = names[j];
                        names[j] = names[j + 1];
                        names[j + 1] = tempName;
                    }
                }
            }

            Console.WriteLine("Траты отсортированы:");

            for (int i = 0; i < count; i++)
            {
                Console.WriteLine($"{names[i]} - {prices[i]:F2} руб.");
            }
            break;

        case 4:
            Console.Write("Введите курс валюты: ");

            if (double.TryParse((Console.ReadLine() ?? "").Replace('.', ','), out double rate) && rate > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    Console.WriteLine($"{names[i]} - {prices[i] / rate:F2}");
                }
            }
            else
            {
                Console.WriteLine("Неверный курс");
            }
            break;

        case 5:
            Console.Write("Введите название: ");
            string search = (Console.ReadLine() ?? "").ToLower();
            bool found = false;

            for (int i = 0; i < count; i++)
            {
                if (names[i].ToLower().Contains(search))
                {
                    Console.WriteLine($"{names[i]} - {prices[i]:F2} руб.");
                    found = true;
                }
            }

            if (!found)
            {
                Console.WriteLine("Ничего не найдено");
            }
            break;

        case 0:
            Console.WriteLine("Выход");
            break;

        default:
            Console.WriteLine("Неверный пунк");
            break;
    }
}
