namespace PracticalWork5;

class Program
{
    private const int AmericanoWater = 300;
    private const int AmericanoPrice = 150;
    private const int LatteWater = 30;
    private const int LatteMilk = 270;
    private const int LattePrice = 170;

    static void Main()
    {
        Console.Write("Введите количество воды (мл): ");
        if (!int.TryParse(Console.ReadLine(), out int water) || water < 0)
        {
            Console.WriteLine("Некорректное количество воды.");
            return;
        }

        Console.Write("Введите количество молока (мл): ");
        if (!int.TryParse(Console.ReadLine(), out int milk) || milk < 0)
        {
            Console.WriteLine("Некорректное количество молока.");
            return;
        }

        int americanoCount = 0;
        int latteCount = 0;
        int money = 0;

        while (CanMakeAnyDrink(water, milk))
        {
            Console.Write("Выберите напиток (1 — американо, 2 — латте, 0 — выход): ");
            string input = Console.ReadLine() ?? string.Empty;

            switch (input)
            {
                case "0":
                    PrintReport(water, milk, americanoCount, latteCount, money);
                    return;

                case "1":
                    if (water >= AmericanoWater)
                    {
                        water -= AmericanoWater;
                        americanoCount++;
                        money += AmericanoPrice;
                        Console.WriteLine("Ваш американо готов.");
                    }
                    else
                    {
                        Console.WriteLine("Не хватает воды для американо.");
                    }
                    break;

                case "2":
                    if (water >= LatteWater && milk >= LatteMilk)
                    {
                        water -= LatteWater;
                        milk -= LatteMilk;
                        latteCount++;
                        money += LattePrice;
                        Console.WriteLine("Ваш латте готов.");
                    }
                    else
                    {
                        Console.WriteLine(water < LatteWater ? "Не хватает воды для латте." : "Не хватает молока для латте.");
                    }
                    break;

                default:
                    Console.WriteLine("Некорректный выбор. Пожалуйста, выберите 1, 2 или 0.");
                    break;
            }
        }

        PrintReport(water, milk, americanoCount, latteCount, money);
    }

    private static bool CanMakeAnyDrink(int water, int milk)
    {
        return water >= AmericanoWater || (water >= LatteWater && milk >= LatteMilk);
    }

    private static void PrintReport(int water, int milk, int americanoCount, int latteCount, int money)
    {
        Console.WriteLine("\n*Отчёт*");
        Console.WriteLine("Ингредиенты подошли к концу");
        Console.WriteLine($"Вода: {water} мл");
        Console.WriteLine($"Молоко: {milk} мл");
        Console.WriteLine($"Кружек американо: {americanoCount}");
        Console.WriteLine($"Кружек латте: {latteCount}");
        Console.WriteLine($"Итого: {money} руб.");
    }
}