namespace PracticalWork7;

class Program
{
    static void Main()
    {
        Console.Write("Введите n: ");
        int n = int.Parse(Console.ReadLine()  ?? string.Empty);
        Console.Write("Введите a: ");
        int a = int.Parse(Console.ReadLine()  ?? string.Empty);
        Console.Write("Введите b: ");
        int b = int.Parse(Console.ReadLine()  ?? string.Empty);
        Console.Write("Введите w: ");
        int w = int.Parse(Console.ReadLine()  ?? string.Empty);
        Console.Write("Введите h: ");
        int h = int.Parse(Console.ReadLine()  ?? string.Empty);

        int left = 0, right = Math.Min(w, h);
        int d = 0;

        while (left <= right)
        {
            int mid = (left + right) / 2;
            long modulesInRow = w / (a + 2 * mid);
            long modulesInCol = h / (b + 2 * mid);

            if (modulesInRow * modulesInCol >= n)
            {
                d = mid;
                left = mid + 1;
            }
            else
            {
                right = mid - 1;
            }
        }

        Console.WriteLine($"Ответ d = {d}");
    }
}