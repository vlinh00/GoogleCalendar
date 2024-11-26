
using System.Text.Json.Serialization;
namespace AT.Share.Model
{
    public class Contact
    {
        [JsonPropertyName("contact_id")]
        public string ContactId { get; set; }

        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }

        [JsonPropertyName("last_name")]
        public string LastName { get; set; }

        [JsonPropertyName("phone_mobile")]
        public string PhoneMobile { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }
    }
}
