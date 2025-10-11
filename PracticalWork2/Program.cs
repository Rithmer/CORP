namespace PracticalWork2;

class Program
{
    static void Main()
    {
        Console.Write("Введите 6-значный номер билета: ");
        string input = Console.ReadLine() ?? string.Empty;
        if (string.IsNullOrEmpty(input) || input.Length != 6 || !int.TryParse(input, out int ticket))
        {
            Console.WriteLine("Ошибка: введите корректный 6-значный номер.");
            return;
        }

        int leftSum = 0, rightSum = 0;

        for (int i = 0; i < 6; i++)
        {
            int digit = ticket % 10;
            ticket /= 10;

            if (i < 3) rightSum += digit;
            else leftSum += digit;
        }

        Console.WriteLine(leftSum == rightSum);
    }
}