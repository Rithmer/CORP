namespace PracticalWork3_2_1
{
    public class Reservation
    {
        public int ClientId { get; private set; }
        public string ClientName { get; private set; }
        public string Phone { get; private set; }
        public string StartTime { get; private set; }
        public string EndTime { get; private set; }
        public string Comment { get; private set; }
        public Table AssignedTable { get; private set; }

        public Reservation(int clientId, string name, string phone, string start, string end, string comment,
            Table table)
        {
            ClientId = clientId;
            ClientName = name;
            Phone = phone;
            StartTime = start;
            EndTime = end;
            Comment = comment;
            AssignedTable = table;
            UpdateSchedule();
        }

        private void UpdateSchedule()
        {
            ClearSchedule();
            var keys = AssignedTable.Schedule.Keys.OrderBy(k => k).ToList();
            bool filling = false;

            foreach (var slot in keys)
            {
                if (slot == StartTime) filling = true;
                if (filling)
                {
                    AssignedTable.Schedule[slot] = $"ID {ClientId}, {ClientName}, {Phone}";
                    if (slot == EndTime) break;
                }
            }
        }

        private void ClearSchedule()
        {
            var keys = AssignedTable.Schedule.Keys.OrderBy(k => k).ToList();
            bool clearing = false;

            foreach (var slot in keys)
            {
                if (slot == StartTime) clearing = true;
                if (clearing)
                {
                    if (AssignedTable.Schedule[slot].Contains($"ID {ClientId}"))
                        AssignedTable.Schedule[slot] = "";
                    if (slot == EndTime) break;
                }
            }
        }

        public void Cancel()
        {
            ClearSchedule();
        }

        public void Edit(string newStart, string newEnd, string newComment, List<Reservation> allReservations)
        {
            int startHour = int.Parse(newStart.Split(':')[0]);
            int endHour = int.Parse(newEnd.Split(':')[0]);

            if (endHour <= startHour)
                throw new ArgumentException("Время окончания должно быть позже начала.");

            string startSlot = $"{startHour:D2}:00-{(startHour + 1):D2}:00";
            string endSlot = $"{(endHour - 1):D2}:00-{endHour:D2}:00";

            var scheduleKeys = AssignedTable.Schedule.Keys.OrderBy(k => k).ToList();
            if (!scheduleKeys.Contains(startSlot) || !scheduleKeys.Contains(endSlot))
                throw new ArgumentException("Указанное время не соответствует доступным слотам (9:00–18:00).");

            foreach (var res in allReservations)
            {
                if (res.AssignedTable.Id != AssignedTable.Id || res.ClientId == ClientId) continue;

                int resStartHour = int.Parse(res.StartTime.Split('-')[0].Split(':')[0]);
                int resEndHour = int.Parse(res.EndTime.Split('-')[1].Split(':')[0]);

                if (startHour < resEndHour && endHour > resStartHour)
                    throw new InvalidOperationException(
                        $"Стол уже забронирован с {res.StartTime.Split('-')[0]} до {res.EndTime.Split('-')[1]}.");
            }

            bool started = false;
            foreach (var slot in scheduleKeys)
            {
                if (slot == startSlot) started = true;
                if (started)
                {
                    if (slot == endSlot) break;
                    if (AssignedTable.Schedule[slot] != "" && !AssignedTable.Schedule[slot].Contains($"ID {ClientId}"))
                        throw new InvalidOperationException("Один из слотов уже занят.");
                }
            }

            ClearSchedule();

            StartTime = startSlot;
            EndTime = endSlot;
            Comment = newComment;

            UpdateSchedule();
        }
    }
}