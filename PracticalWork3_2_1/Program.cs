namespace PracticalWork3_2_1
{
    class Program
    {
        static void Main()
        {
            List<Table> tables = new();
            List<Reservation> reservations = new();

            while (true)
            {
                Console.WriteLine("\n=== Система бронирования ===");
                Console.WriteLine("1. Создать стол");
                Console.WriteLine("2. Изменить данные стола");
                Console.WriteLine("3. Вывести информацию о столе");
                Console.WriteLine("4. Создать бронирование");
                Console.WriteLine("5. Отменить бронирование");
                Console.WriteLine("6. Показать все бронирования");
                Console.WriteLine("7. Поиск брони по имени и 4 цифрам телефона");
                Console.WriteLine("8. Изменить бронирование");
                Console.WriteLine("9. Показать доступные столы");
                Console.WriteLine("0. Выход");
                Console.Write("Выберите действие: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": CreateTable(tables); break;
                    case "2": EditTable(tables, reservations); break;
                    case "3": ShowTableInfo(tables); break;
                    case "4": CreateReservation(tables, reservations); break;
                    case "5": CancelReservation(reservations); break;
                    case "6": ShowAllReservations(reservations); break;
                    case "7": SearchReservation(reservations); break;
                    case "8": EditReservation(reservations); break;
                    case "9": ShowAvailableTables(tables, reservations); break;
                    case "0": return;
                    default: Console.WriteLine("Неверный выбор."); break;
                }
            }
        }

        static void CreateTable(List<Table> tables)
        {
            Console.Write("ID: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Неверный ID.");
                return;
            }

            Console.Write("Расположение: ");
            string loc = Console.ReadLine();
            Console.Write("Количество мест: ");
            if (!int.TryParse(Console.ReadLine(), out int seats) || seats <= 0)
            {
                Console.WriteLine("Неверное количество мест.");
                return;
            }

            if (tables.Any(t => t.Id == id))
            {
                Console.WriteLine("Стол с таким ID уже существует.");
                return;
            }

            tables.Add(new Table(id, loc, seats));
            Console.WriteLine("Стол успешно создан.");
        }

        static void EditTable(List<Table> tables, List<Reservation> reservations)
        {
            Console.Write("Введите ID стола: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Неверный ID.");
                return;
            }

            var table = tables.FirstOrDefault(t => t.Id == id);
            if (table == null)
            {
                Console.WriteLine("Стол не найден.");
                return;
            }

            bool active = reservations.Any(r => r.AssignedTable.Id == id);
            if (active)
            {
                Console.WriteLine("Стол находится в активном бронировании. Изменение невозможно.");
                return;
            }

            Console.Write("Новое расположение: ");
            string loc = Console.ReadLine();
            Console.Write("Новое количество мест: ");
            if (!int.TryParse(Console.ReadLine(), out int seats) || seats <= 0)
            {
                Console.WriteLine("Неверное количество мест.");
                return;
            }

            table.EditInfo(loc, seats);
            Console.WriteLine("Информация о столе обновлена.");
        }

        static void ShowTableInfo(List<Table> tables)
        {
            Console.Write("Введите ID стола: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Неверный ID.");
                return;
            }

            var table = tables.FirstOrDefault(t => t.Id == id);
            if (table != null)
                table.PrintInfo();
            else
                Console.WriteLine("Стол не найден.");
        }

        static void CreateReservation(List<Table> tables, List<Reservation> reservations)
        {
            Console.Write("Введите ID стола: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Неверный ID.");
                return;
            }

            var table = tables.FirstOrDefault(t => t.Id == id);
            if (table == null)
            {
                Console.WriteLine("Стол не найден.");
                return;
            }

            Console.Write("ID клиента: ");
            if (!int.TryParse(Console.ReadLine(), out int cid))
            {
                Console.WriteLine("Неверный ID клиента.");
                return;
            }

            Console.Write("Имя клиента: ");
            string name = Console.ReadLine();
            Console.Write("Телефон: ");
            string phone = Console.ReadLine();
            Console.Write("Начало брони (например, 12:00): ");
            string startTime = Console.ReadLine().Trim();
            Console.Write("Окончание брони (например, 15:00): ");
            string endTime = Console.ReadLine().Trim();
            Console.Write("Комментарий: ");
            string comment = Console.ReadLine();

            try
            {
                int startHour = int.Parse(startTime.Split(':')[0]);
                int endHour = int.Parse(endTime.Split(':')[0]);

                if (endHour <= startHour)
                {
                    Console.WriteLine("Ошибка: Время окончания должно быть позже начала.");
                    return;
                }

                string startSlot = $"{startHour:D2}:00-{(startHour + 1):D2}:00";
                string endSlot = $"{(endHour - 1):D2}:00-{endHour:D2}:00";

                var scheduleKeys = table.Schedule.Keys.OrderBy(k => k).ToList();
                if (!scheduleKeys.Contains(startSlot) || !scheduleKeys.Contains(endSlot))
                {
                    Console.WriteLine("Ошибка: Указанное время не соответствует доступным слотам (9:00–18:00).");
                    return;
                }
                
                foreach (var res in reservations)
                {
                    if (res.AssignedTable.Id != id) continue;

                    int resStartHour = int.Parse(res.StartTime.Split('-')[0].Split(':')[0]);
                    int resEndHour = int.Parse(res.EndTime.Split('-')[1].Split(':')[0]);

                    if (startHour < resEndHour && endHour > resStartHour)
                    {
                        Console.WriteLine($"Ошибка: Стол уже забронирован с {res.StartTime.Split('-')[0]} до {res.EndTime.Split('-')[1]} (ID {res.ClientId}).");
                        return;
                    }
                }
                
                bool started = false;
                foreach (var slot in scheduleKeys)
                {
                    if (slot == startSlot) started = true;
                    if (started && table.Schedule[slot] != "")
                    {
                        Console.WriteLine("Ошибка: Один или несколько слотов уже заняты.");
                        return;
                    }
                    if (started && slot == endSlot) break;
                }

                var reservation = new Reservation(cid, name, phone, startSlot, endSlot, comment, table);
                reservations.Add(reservation);
                Console.WriteLine("Бронирование успешно добавлено.");
            }
            catch (FormatException)
            {
                Console.WriteLine("Ошибка: Неверный формат времени. Используйте ЧЧ:ММ.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        static void CancelReservation(List<Reservation> reservations)
        {
            Console.Write("Введите ID клиента: ");
            if (!int.TryParse(Console.ReadLine(), out int cid))
            {
                Console.WriteLine("Неверный ID.");
                return;
            }

            var r = reservations.FirstOrDefault(x => x.ClientId == cid);
            if (r == null)
            {
                Console.WriteLine("Бронирование не найдено.");
                return;
            }

            r.Cancel();
            reservations.Remove(r);
            Console.WriteLine("Бронирование отменено.");
        }

        static void ShowAllReservations(List<Reservation> reservations)
        {
            if (reservations.Count == 0)
            {
                Console.WriteLine("Нет активных бронирований.");
                return;
            }

            foreach (var r in reservations)
            {
                string start = r.StartTime.Split('-')[0];
                string end = r.EndTime.Split('-')[1]; // Правильное время окончания
                Console.WriteLine($"ID {r.ClientId}: {r.ClientName}, {r.Phone}, {start}-{end}, стол {r.AssignedTable.Id}, комментарий: {r.Comment}");
            }
        }

        static void SearchReservation(List<Reservation> reservations)
        {
            Console.Write("Имя клиента: ");
            string name = Console.ReadLine();
            Console.Write("Последние 4 цифры телефона: ");
            string last4 = Console.ReadLine();

            var found = reservations.FirstOrDefault(r => 
                r.ClientName.Equals(name, StringComparison.OrdinalIgnoreCase) && 
                r.Phone.EndsWith(last4));

            if (found != null)
            {
                string start = found.StartTime.Split('-')[0];
                string end = found.EndTime.Split('-')[1];
                Console.WriteLine($"Найдено: {found.ClientName}, {found.Phone}, Стол {found.AssignedTable.Id}, {start}-{end}");
            }
            else
            {
                Console.WriteLine("Бронирование не найдено.");
            }
        }

        static void EditReservation(List<Reservation> reservations)
        {
            Console.Write("Введите ID клиента для изменения: ");
            if (!int.TryParse(Console.ReadLine(), out int cid))
            {
                Console.WriteLine("Неверный ID.");
                return;
            }

            var reservation = reservations.FirstOrDefault(r => r.ClientId == cid);
            if (reservation == null)
            {
                Console.WriteLine("Бронирование не найдено.");
                return;
            }

            string currentStart = reservation.StartTime.Split('-')[0];
            string currentEnd = reservation.EndTime.Split('-')[1];
            Console.WriteLine($"Текущее: {currentStart} - {currentEnd}, комментарий: {reservation.Comment}");

            Console.Write("Новое начало (например, 13:00): ");
            string newStart = Console.ReadLine().Trim();
            Console.Write("Новое окончание (например, 16:00): ");
            string newEnd = Console.ReadLine().Trim();
            Console.Write("Новый комментарий: ");
            string newComment = Console.ReadLine();

            try
            {
                reservation.Edit(newStart, newEnd, newComment, reservations);
                Console.WriteLine("Бронирование успешно изменено.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        static void ShowAvailableTables(List<Table> tables, List<Reservation> reservations)
        {
            Console.Write("Минимальное количество мест (0 — без фильтра): ");
            int minSeats = int.TryParse(Console.ReadLine(), out int ms) ? ms : 0;

            Console.Write("Начало поиска (например, 12:00, или пусто): ");
            string startInput = Console.ReadLine().Trim();
            Console.Write("Окончание поиска (например, 15:00, или пусто): ");
            string endInput = Console.ReadLine().Trim();

            var available = tables.Where(t => minSeats == 0 || t.Seats >= minSeats).ToList();

            if (!string.IsNullOrEmpty(startInput) && !string.IsNullOrEmpty(endInput))
            {
                try
                {
                    int startHour = int.Parse(startInput.Split(':')[0]);
                    int endHour = int.Parse(endInput.Split(':')[0]);
                    if (endHour <= startHour) throw new Exception();

                    string startSlot = $"{startHour:D2}:00-{(startHour + 1):D2}:00";
                    string endSlot = $"{(endHour - 1):D2}:00-{endHour:D2}:00";

                    available = available.Where(t => t.IsAvailable(startSlot, endSlot)).ToList();
                }
                catch
                {
                    Console.WriteLine("Ошибка в формате времени поиска.");
                    return;
                }
            }

            if (!available.Any())
            {
                Console.WriteLine("Нет доступных столов по заданным критериям.");
                return;
            }

            Console.WriteLine("\nДоступные столы:");
            foreach (var table in available)
            {
                Console.WriteLine($"ID: {table.Id:D2}, Мест: {table.Seats}, Расположение: {table.Location}");
            }
        }
    }
}