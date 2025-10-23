namespace PracticalWork3_1_1;

class Program
{
    static void Main()
    {
        Console.Write("Введите число: ");
        int n = int.Parse(Console.ReadLine());
        int reversed = ReverseNumber(n, 0);
        Console.WriteLine($"Разворот числа: {reversed}");
    }
    
    static int ReverseNumber(int n, int result)
    {
        if (n == 0)
            return result;

        return ReverseNumber(n / 10, result * 10 + n % 10);
    }
}
