namespace PracticalWork3_1_2;

class Program
{
    static void Main()
    {
        Console.Write("Введите m: ");
        int m = int.Parse(Console.ReadLine());
        Console.Write("Введите n: ");
        int n = int.Parse(Console.ReadLine());

        int result = Ackermann(m, n);
        Console.WriteLine($"A({m},{n}) = {result}");
    }
    
    static int Ackermann(int m, int n)
    {
        if (m == 0)
            return n + 1;
        else if (m > 0 && n == 0)
            return Ackermann(m - 1, 1);
        else // m > 0 && n > 0
            return Ackermann(m - 1, Ackermann(m, n - 1));
    }
}
