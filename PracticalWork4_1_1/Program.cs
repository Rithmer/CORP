namespace PracticalWork4_1_1;

class Program
{
    static List<Dish> menu = new();
    static List<Order> orders = new();

    static void Main()
    {
        Console.WriteLine("=== Система заказов ===");

        while (true)
        {
            Console.WriteLine("\n1 - Создание заказа");
            Console.WriteLine("2 - Изменение заказа (добавление/удаление блюда)");
            Console.WriteLine("3 - Вывод информации о заказе");
            Console.WriteLine("4 - Закрытие заказа");
            Console.WriteLine("5 - Вывод чека (только для закрытых заказов)");
            Console.WriteLine("6 - Добавить блюдо в меню");
            Console.WriteLine("7 - Показать меню");
            Console.WriteLine("8 - Редактировать блюдо в меню");
            Console.WriteLine("9 - Удалить блюдо из меню");
            Console.WriteLine("10 - Вывод информации о блюде");
            Console.WriteLine("11 - Подсчет стоимости всех закрытых заказов");
            Console.WriteLine("12 - Подсчет закрытых заказов официанта");
            Console.WriteLine("13 - Статистика по заказанным блюдам");
            Console.WriteLine("0 - Выход");
            Console.Write("Выберите действие: ");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1": CreateOrder(); break;
                case "2": EditExistingOrder(); break;
                case "3": ShowOrderInfo(); break;
                case "4": CloseOrder(); break;
                case "5": PrintOrderReceipt(); break;
                case "6": AddDishToMenu(); break;
                case "7": PrintMenu(); break;
                case "8": EditDishInMenu(); break;
                case "9": DeleteDishFromMenu(); break;
                case "10": PrintDishInfo(); break;
                case "11": CalculateTotalClosedOrdersCost(); break;
                case "12": CountClosedOrdersForWaiter(); break;
                case "13": PrintDishStatistics(); break;
                case "0": return;
                default: Console.WriteLine("Неверный выбор."); break;
            }
        }
    }

    static void AddDishToMenu()
    {
        Console.Write("\nВведите Id блюда: ");
        int id = int.Parse(Console.ReadLine());
        Console.Write("Название: ");
        string name = Console.ReadLine();
        Console.Write("Состав: ");
        string composition = Console.ReadLine();
        Console.Write("Вес (100/20/50): ");
        string weight = Console.ReadLine();
        Console.Write("Цена: ");
        double price = double.Parse(Console.ReadLine());
        Console.Write("Категория (0-Напитки, 1-Салаты, 2-ХолодныеЗакуски, 3-ГорячиеЗакуски, 4-Супы, 5-ГорячиеБлюда, 6-Десерты): ");
        Category category = (Category)int.Parse(Console.ReadLine());
        Console.Write("Время готовки: ");
        int cookTime = int.Parse(Console.ReadLine());
        Console.Write("Типы (через запятую): ");
        string[] type = Console.ReadLine().Split(',');

        Dish d = new(id, name, composition, weight, price, category, cookTime, type);
        menu.Add(d);
        Console.WriteLine("Блюдо добавлено.");
    }

    static void PrintMenu()
    {
        Console.WriteLine("\n=== МЕНЮ ===");
        foreach (var g in menu.GroupBy(d => d.Category))
        {
            Console.WriteLine($"\n{g.Key}:");
            foreach (var d in g)
                Console.WriteLine($"{d.Id}. {d.Name} — {d.Price}₽ ({d.Weight}, время: {d.CookTime} мин., тип: {string.Join(", ", d.Type)})");
        }
    }

    static void CreateOrder()
    {
        if (menu.Count == 0)
        {
            Console.WriteLine("Ошибка: меню пустое. Добавьте блюда в меню перед созданием заказа.");
            return;
        }

        Console.Write("Введите Id заказа: ");
        int id = int.Parse(Console.ReadLine());
        Console.Write("Введите Id стола: ");
        int tableId = int.Parse(Console.ReadLine());
        Console.Write("Введите Id официанта: ");
        int waiter = int.Parse(Console.ReadLine());
        Console.Write("Комментарий: ");
        string comment = Console.ReadLine();

        Console.Write("Введите количество блюд в заказе: ");
        int count = int.Parse(Console.ReadLine());
        if (count <= 0)
        {
            Console.WriteLine("Количество блюд должно быть > 0.");
            return;
        }
        List<Dish> orderDishes = new();

        for (int i = 0; i < count; i++)
        {
            Console.Write($"Введите Id блюда #{i + 1}: ");
            int dishId = int.Parse(Console.ReadLine());
            Dish found = menu.FirstOrDefault(d => d.Id == dishId);
            if (found != null)
                orderDishes.Add(found);
            else
                Console.WriteLine("Такого блюда нет в меню.");
        }

        if (orderDishes.Count == 0)
        {
            Console.WriteLine("Заказ не создан: не выбрано ни одного блюда.");
            return;
        }

        Order o = new(id, tableId, waiter, comment, orderDishes);
        orders.Add(o);
        Console.WriteLine($"Заказ #{id} создан и содержит {orderDishes.Count} блюд.");
    }

    static void EditExistingOrder()
    {
        Console.Write("Введите Id заказа: ");
        int id = int.Parse(Console.ReadLine());
        Order order = orders.FirstOrDefault(o => o.Id == id);
        if (order == null) { Console.WriteLine("Такого заказа нет."); return; }
    
        if (order.CloseTime != null)
        {
            Console.WriteLine("Ошибка: заказ закрыт и не может быть изменен.");
            return;
        }

        Console.WriteLine("1 - Добавить блюдо");
        Console.WriteLine("2 - Удалить блюдо");
        string subChoice = Console.ReadLine();

        if (subChoice == "1")
        {
            Console.Write("Введите Id блюда для добавления: ");
            int dishId = int.Parse(Console.ReadLine());
            Dish d = menu.FirstOrDefault(x => x.Id == dishId);
            if (d == null) { Console.WriteLine("Такого блюда нет."); return; }
            order.EditOrder(d);
        }
        else if (subChoice == "2")
        {
            Console.Write("Введите Id блюда для удаления: ");
            int dishId = int.Parse(Console.ReadLine());
            order.RemoveDishFromOrder(dishId);
        }
        else
        {
            Console.WriteLine("Неверный выбор.");
        }
    }

    static void ShowOrderInfo()
    {
        Console.Write("Введите Id заказа: ");
        int id = int.Parse(Console.ReadLine());
        orders.FirstOrDefault(o => o.Id == id)?.PrintInfo();
    }

    static void CloseOrder()
    {
            
        Console.Write("Введите Id заказа: ");
        int id = int.Parse(Console.ReadLine());
        Order o = orders.FirstOrDefault(x => x.Id == id);
        if (o == null) { Console.WriteLine("Такого заказа нет."); return; }
        o.CloseOrder();
    }

    static void PrintOrderReceipt()
    {
        Console.Write("Введите Id заказа: ");
        int id = int.Parse(Console.ReadLine());
        Order o = orders.FirstOrDefault(x => x.Id == id);
        if (o == null) { Console.WriteLine("Такого заказа нет."); return; }
        o.PrintReceipt();
    }

    static void EditDishInMenu()
    {
        Console.Write("Введите Id блюда: ");
        int id = int.Parse(Console.ReadLine());
        Dish d = menu.FirstOrDefault(x => x.Id == id);
        if (d == null) { Console.WriteLine("Такого блюда нет."); return; }

        Console.Write("Новое название: ");
        string newName = Console.ReadLine();
        Console.Write("Новая цена: ");
        double newPrice = double.Parse(Console.ReadLine());

        d.EditDish(ref newName, ref newPrice);
    }

    static void DeleteDishFromMenu()
    {
        Console.Write("Введите Id блюда: ");
        int id = int.Parse(Console.ReadLine());
        Dish d = menu.FirstOrDefault(x => x.Id == id);
        if (d == null) { Console.WriteLine("Такого блюда нет."); return; }

        d.DeleteDish();
        menu.Remove(d);
    }

    static void PrintDishInfo()
    {
        Console.Write("Введите Id блюда: ");
        int id = int.Parse(Console.ReadLine());
        menu.FirstOrDefault(d => d.Id == id)?.PrintInfo();
    }

    static void CalculateTotalClosedOrdersCost()
    {
        double total;
        GetTotalClosedCost(out total);
        Console.WriteLine($"Общая стоимость закрытых заказов: {total}₽");
    }
    
    static void GetTotalClosedCost(out double total)
    {
        total = orders.Where(o => o.CloseTime != null).Sum(o => o.TotalCost);
    }

    static void CountClosedOrdersForWaiter()
    {
        Console.Write("Введите Id официанта: ");
        int waiterId = int.Parse(Console.ReadLine());
        int count = orders.Count(o => o.CloseTime != null && o.WaiterId == waiterId);
        Console.WriteLine($"Закрытых заказов у официанта {waiterId}: {count}");
    }

    static void PrintDishStatistics()
    {
        var stats = orders.SelectMany(o => o.Dishes)
            .GroupBy(d => d.Id)
            .Select(g => new { Dish = menu.FirstOrDefault(d => d.Id == g.Key), Count = g.Count() })
            .Where(s => s.Dish != null);

        Console.WriteLine("\n=== Статистика по блюдам ===");
        foreach (var s in stats)
        {
            double threshold = 1000.0;
            bool expensive = s.Dish.IsMoreExpensiveThan(in threshold);
            string expensiveText = expensive ? "Да" : "Нет";
            Console.WriteLine($"{s.Dish.Name} (Id: {s.Dish.Id}) заказано {s.Count} раз (дорогое: {expensiveText})");
        }
    }
}