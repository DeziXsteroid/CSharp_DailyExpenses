Console.Write("Количество трат от 2 до 40: ");
int count = Convert.ToInt32(Console.ReadLine());

string[] names = new string[count];
double[] prices = new double[count];

for (int i = 0; i < count; i++)
{
    Console.Write("Название; Сумма: ");
    string[] data = Console.ReadLine().Split(';');
    names[i] = data[0];
    prices[i] = Convert.ToDouble(data[1]);
}

int menu = -1;

while (menu != 0)
{
    Console.WriteLine("\n1. Вывод данных");
    Console.WriteLine("2. Статистика");
    Console.WriteLine("3. Сортировка по цене");
    Console.WriteLine("4. Конвертация валюты");
    Console.WriteLine("5. Поиск по названию");
    Console.WriteLine("0. Выход");
    menu = Convert.ToInt32(Console.ReadLine());

    if (menu == 1)
    {
        for (int i = 0; i < count; i++)
            Console.WriteLine($"{names[i]} - {prices[i]} руб.");
    }

    if (menu == 2)
    {
        double sum = 0;
        double min = prices[0];
        double max = prices[0];

        for (int i = 0; i < count; i++)
        {
            sum += prices[i];
            if (prices[i] < min) min = prices[i];
            if (prices[i] > max) max = prices[i];
        }

        Console.WriteLine($"Сумма: {sum}");
        Console.WriteLine($"Среднее: {sum / count}");
        Console.WriteLine($"Минимальное: {min}");
        Console.WriteLine($"Максимальное: {max}");
    }

    if (menu == 3)
    {
        for (int i = 0; i < count - 1; i++)
        {
            for (int j = 0; j < count - i - 1; j++)
            {
                if (prices[j] > prices[j + 1])
                {
                    double price = prices[j];
                    prices[j] = prices[j + 1];
                    prices[j + 1] = price;

                    string name = names[j];
                    names[j] = names[j + 1];
                    names[j + 1] = name;
                }
            }
        }

        for (int i = 0; i < count; i++)
            Console.WriteLine($"{names[i]} - {prices[i]} руб.");
    }

    if (menu == 4)
    {
        Console.Write("Курс валюты: ");
        double rate = Convert.ToDouble(Console.ReadLine());

        for (int i = 0; i < count; i++)
            Console.WriteLine($"{names[i]} - {prices[i] / rate:F2}");
    }

    if (menu == 5)
    {
        Console.Write("Название: ");
        string search = Console.ReadLine();

        for (int i = 0; i < count; i++)
        {
            if (names[i].Contains(search))
                Console.WriteLine($"{names[i]} - {prices[i]} руб.");
        }
    }
}
