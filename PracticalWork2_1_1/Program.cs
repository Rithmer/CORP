namespace PracticalWork1;

static class Program
{
    static void Main()
    {
        Console.Write("Введите x: ");
        double x = Convert.ToDouble(Console.ReadLine());

        Console.Write("Введите точность e (<0.01): ");
        double eps = Convert.ToDouble(Console.ReadLine());
        if (eps <= 0 || eps >= 0.01)
        {
            Console.WriteLine("Ошибка: точность должна быть больше 0 и меньше 0.01.");
            eps = 0.001;
        }

        double sum = 1;

        for (int i = 1; ; i++)
        {
            double term = Math.Pow(-1, i) * Math.Pow(x, 2 * i) / Factorial(2 * i);
            if (Math.Abs(term) <= eps) break;
            sum += term;
        }

        Console.WriteLine($"cos({x}) ≈ {sum}");

        Console.Write("Введите n для n-го члена ряда: ");
        int k = Convert.ToInt32(Console.ReadLine());

        if (k <= 0)
        {
            Console.WriteLine("Ошибка: n должно быть положительным.");
        }
        else
        {
            int index = k - 1;
            double nth = Math.Pow(-1, index) * Math.Pow(x, 2 * index) / Factorial(2 * index);
            Console.WriteLine($"{k}-й член ряда = {nth}");
        }
    }

    static double Factorial(int n)
    {
        double res = 1;
        for (int i = 1; i <= n; i++) res *= i;
        return res;
    }
}