namespace PracticalWork6;

class Program
{
    static void Main()
    {
        Console.Write("Введите количество бактерий: ");
        int n = int.Parse(Console.ReadLine() ?? string.Empty);
        Console.Write("Введите количество антибиотика: ");
        int x = int.Parse(Console.ReadLine() ?? string.Empty);

        int hour = 0;

        while (n > 0 && hour < 10)
        {
            hour++;
            n *= 2;

            int kill = x * (11 - hour); 

            n -= kill;

            if (n <= 0)
            {
                Console.WriteLine($"После {hour} часа бактерий осталось 0");
                Console.WriteLine("Эксперимент завершён.");
                return;
            }

            Console.WriteLine($"После {hour} часа бактерий осталось {n}");
        }

        if (hour == 10)
            Console.WriteLine("Антибиотик перестал действовать.");
    }
}