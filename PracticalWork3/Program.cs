namespace PracticalWork3;

class Program
{
    static void Main()
    {
        Console.Write("Введите числитель: ");
        int m = int.Parse(Console.ReadLine() ?? string.Empty);

        Console.Write("Введите знаменатель: ");
        int n = int.Parse(Console.ReadLine() ?? string.Empty);


        if (n == 0)
        {
            Console.WriteLine("Знаменатель не может быть равен 0");
            return;
        }


        int absM = Math.Abs(m);
        int absN = Math.Abs(n);
        int a = absM, b = absN, temp;


        while (b != 0)
        {
            temp = a % b;
            a = b;
            b = temp;
        }

        int gcd = a;


        int reducedM = m / gcd;
        int reducedN = n / gcd;


        if (reducedN < 0)
        {
            reducedM = -reducedM;
            reducedN = -reducedN;
        }

        Console.WriteLine($"Результат: {reducedM} / {reducedN}"); 
    }
}