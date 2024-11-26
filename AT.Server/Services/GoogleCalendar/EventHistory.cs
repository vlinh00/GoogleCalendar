using AT.Share.Model;
namespace AT.Server.Services.GoogleCalendar
{
    public class EventHistory
    {
        public string EventId { get; set; } // ID duy nhất của sự kiện
        //public string Description { get; set; } // nội dung sự kiện
        public List<DescriptionUpdate> UpdateHistory { get; private set; } // Danh sách các cập nhật (Description + Timestamp)
        //public List<DateTime> UpdateTimestamps { get; private set; } // Danh sách các thời gian cập nhật


        public EventHistory(string eventId, string initialDescription, DateTime dateTime)
        {
            EventId = eventId;
            UpdateHistory = new List<DescriptionUpdate>
        {
            new DescriptionUpdate(initialDescription, dateTime) // Lưu cập nhật đầu tiên
        };
        }

        // Phương thức thêm cập nhật mới
        public void AddUpdateTimestamp(string newDescription, DateTime dateTime)
        {
            // Thêm cặp mô tả và thời gian mới vào lịch sử cập nhật
            UpdateHistory.Add(new DescriptionUpdate(newDescription, dateTime));
        }

        //public EventHistory(string eventId, string description)
        //{
        //    EventId = eventId;
        //    Description = description;
        //    UpdateTimestamps = new List<DateTime> { DateTime.UtcNow };
        //}

        //// Phương thức thêm thời gian cập nhật mới
        //public void AddUpdateTimestamp(string description)
        //{
        //    Description += description;
        //    UpdateTimestamps.Add(DateTime.UtcNow);
        //}
    }
}
