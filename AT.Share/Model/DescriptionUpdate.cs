using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace AT.Share.Model
{
    // Lớp phụ để lưu cặp Description và Timestamp
    public class DescriptionUpdate
    {
        public string Description { get; set; } // Mô tả tại thời điểm cập nhật
        public DateTime? Timestamp { get; set; }  // Thời gian cập nhật

        public DescriptionUpdate(string description, DateTime? timestamp)
        {
            Description = description;
            Timestamp = timestamp;
        }
    }
}
