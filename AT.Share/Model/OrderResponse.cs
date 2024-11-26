using System.Text.Json.Serialization;

namespace AT.Share.Model
{
    public class OrderResponse // Lớp để chứa danh sách đơn hàng
    {
        [JsonPropertyName("records")]
        public List<Order> Records { get; set; }
    }
}
