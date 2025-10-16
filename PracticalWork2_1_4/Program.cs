namespace PracticalWork4;

class Program
{
    static void Main()
    {

        Console.WriteLine("Загадайте число от 0 до 63.");
        int low = 0, high = 63;

        for (int i = 0; i < 6; i++)
        {
            int mid = (low + high) / 2;
            Console.Write($"Ваше число больше {mid}? (1 - да, 0 - нет): ");
            int ans = int.Parse(Console.ReadLine()  ?? string.Empty);
            if (ans == 1) low = mid + 1;
            else high = mid;
        }

        Console.WriteLine($"Ваше число: {low}");
    }
}