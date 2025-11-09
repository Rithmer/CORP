namespace PracticalWork4_1_1;

public class Order
{
    private int id;
    private int tableId;
    private List<Dish> dishes = new();
    private string comment;
    private string openTime;
    private string closeTime;
    private int waiterId;
    private double totalCost;

    public int Id { get => id; set => id = value; }
    public int TableId { get => tableId; set => tableId = value; }
    public List<Dish> Dishes { get => dishes; set => dishes = value; }
    public string Comment { get => comment; set => comment = value; }
    public string OpenTime { get => openTime; set => openTime = value; }
    public string CloseTime { get => closeTime; set => closeTime = value; }
    public int WaiterId { get => waiterId; set => waiterId = value; }
    public double TotalCost { get => totalCost; set => totalCost = value; }

    public Order(int id, int tableId, int waiterId, string comment, List<Dish> dishes)
    {
        Id = id;
        TableId = tableId;
        WaiterId = waiterId;
        Comment = comment;
        Dishes = dishes;
        OpenTime = DateTime.Now.ToString("HH:mm:ss");
    }

    public void EditOrder(Dish dish)
    {
        Dishes.Add(dish);
        Console.WriteLine($"Блюдо '{dish.Name}' добавлено в заказ #{Id}");
    }

    public void RemoveDishFromOrder(int dishId)
    {
        var dishToRemove = Dishes.FirstOrDefault(d => d.Id == dishId);
        if (dishToRemove != null)
        {
            Dishes.Remove(dishToRemove);
            Console.WriteLine($"Блюдо '{dishToRemove.Name}' удалено из заказа #{Id}");
        }
        else
        {
            Console.WriteLine("Такого блюда в заказе нет.");
        }
    }

    public void PrintInfo()
    {
        Console.WriteLine($"\n=== Заказ #{Id} ===");
        Console.WriteLine($"Столик: {TableId}, официант: {WaiterId}");
        Console.WriteLine($"Комментарий: {Comment}");
        Console.WriteLine($"Принят: {OpenTime}");
        Console.WriteLine("Блюда:");
        foreach (var d in Dishes)
            Console.WriteLine($"- {d.Name} ({d.Price}₽)");
        Console.WriteLine($"Состояние: {(CloseTime == null ? "Открыт" : $"Закрыт ({CloseTime})")}");
    }

    public void CloseOrder()
    {
        if (CloseTime != null)
        {
            Console.WriteLine("Заказ уже закрыт.");
            return;
        }
        CloseTime = DateTime.Now.ToString("HH:mm:ss");
        TotalCost = Dishes.Sum(d => d.Price);
        Console.WriteLine($"Заказ #{Id} закрыт. Итоговая сумма: {TotalCost}₽");
    }

    public void PrintReceipt()
    {
        if (CloseTime == null)
        {
            Console.WriteLine("Ошибка: заказ не закрыт!");
            return;
        }

        Console.WriteLine("\n*************************************************");
        Console.WriteLine($"Столик: {TableId}");
        Console.WriteLine($"Официант: {WaiterId}");
        Console.WriteLine($"Период обслуживания: с {OpenTime} по {CloseTime}");

        var grouped = Dishes.GroupBy(d => d.Category);
        foreach (var g in grouped)
        {
            Console.WriteLine($"\nКатегория {g.Key}:");
            var dishGroups = g.GroupBy(d => d.Id);
            foreach (var dg in dishGroups)
            {
                var first = dg.First();
                int count = dg.Count();
                double subtotal = count * first.Price;
                Console.WriteLine($"{first.Name} {count}*{first.Price}= {subtotal}");
            }
            Console.WriteLine($"Под_итог категории: {g.Sum(d => d.Price)}");
        }

        Console.WriteLine($"\nИтог счета: {TotalCost}₽");
        Console.WriteLine("*************************************************");
    }
}