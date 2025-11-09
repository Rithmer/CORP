namespace PracticalWork4_1_1;

public enum Category
{
    Напитки, Салаты, ХолодныеЗакуски, ГорячиеЗакуски, Супы, ГорячиеБлюда, Десерты
}

public class Dish
{
    private int id;
    private string name;
    private string composition;
    private string weight;
    private double price;
    private Category category;
    private int cookTime;
    private string[] type;

    public int Id { get => id; set => id = value; }
    public string Name { get => name; set => name = value; }
    public string Composition { get => composition; set => composition = value; }
    public string Weight { get => weight; set => weight = value; }
    public double Price { get => price; set => price = value; }
    public Category Category { get => category; set => category = value; }
    public int CookTime { get => cookTime; set => cookTime = value; }
    public string[] Type { get => type; set => type = value; }

    public Dish(int id, string name, string composition, string weight,
        double price, Category category, int cookTime, params string[] type)
    {
        Id = id;
        Name = name;
        Composition = composition;
        Weight = weight;
        Price = price;
        Category = category;
        CookTime = cookTime;
        Type = type;
    }

    public void EditDish(ref string newName, ref double newPrice)
    {
        Name = newName;
        Price = newPrice;
        Console.WriteLine($"Блюдо [{Id}] изменено: {Name}, {Price}₽");
    }

    public void PrintInfo()
    {
        Console.WriteLine($"\nБлюдо #{Id}: {Name}");
        Console.WriteLine($"Категория: {Category}");
        Console.WriteLine($"Состав: {Composition}");
        Console.WriteLine($"Вес: {Weight}");
        Console.WriteLine($"Цена: {Price}₽");
        Console.WriteLine($"Время готовки: {CookTime} мин.");
        Console.WriteLine($"Тип: {string.Join(", ", Type)}");
    }

    public void DeleteDish()
    {
        Console.WriteLine($"Блюдо #{Id} ({Name}) удалено.");
    }
    
    public bool IsMoreExpensiveThan(in double otherPrice)
    {
        return Price > otherPrice;
    }
}