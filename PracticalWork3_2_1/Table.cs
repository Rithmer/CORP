namespace PracticalWork3_2_1
{
    public class Table
    {
        public int Id { get; private set; }
        public string Location { get; private set; }
        public int Seats { get; private set; }
        public Dictionary<string, string> Schedule { get; private set; }

        public Table(int id, string location, int seats)
        {
            Id = id;
            Location = location;
            Seats = seats;
            Schedule = new Dictionary<string, string>();

            for (int h = 9; h < 18; h++)
            {
                string slot = $"{h:D2}:00-{(h + 1):D2}:00";
                Schedule[slot] = "";
            }
        }

        public void EditInfo(string location, int seats)
        {
            Location = location;
            Seats = seats;
        }

        public bool IsAvailable(string startSlot, string endSlot)
        {
            if (!Schedule.ContainsKey(startSlot) || !Schedule.ContainsKey(endSlot)) return false;

            var keys = Schedule.Keys.OrderBy(k => k).ToList();
            bool started = false;
            foreach (var slot in keys)
            {
                if (slot == startSlot) started = true;
                if (started && Schedule[slot] != "") return false;
                if (started && slot == endSlot) break;
            }

            return true;
        }

        public void PrintInfo()
        {
            const int totalWidth = 79;
            Console.WriteLine(new string('*', totalWidth));
            
            PrintLine("ID:", $"{Id:D2}.");
            
            PrintLine("Расположение:", $"«{Location}».");
            
            PrintLine("Количество мест:", Seats.ToString());

            Console.WriteLine("Расписание:");
            foreach (var slot in Schedule.OrderBy(k => k.Key))
            {
                string info = string.IsNullOrEmpty(slot.Value) ? "" : $" {slot.Value}";
                PrintLine(slot.Key, info);
            }

            Console.WriteLine(new string('*', totalWidth));
        }

        private void PrintLine(string left, string right)
        {
            const int totalWidth = 79;
            int dashCount = totalWidth - left.Length - right.Length - 1;
            if (dashCount < 0) dashCount = 0;
            Console.WriteLine($"{left} {new string('-', dashCount)}{right}");
        }
    }
}