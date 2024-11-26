using System.Text.Json.Serialization;

namespace AT.Share.Model
{
    public class Account
    {
        [JsonPropertyName("info")]
        public AccountInfoDeatail Info { get; set; }

        [JsonPropertyName("contacts")]
        public List<ContactDetail> Contacts { get; set; }
    }

    public class AccountInfoDeatail
    {
        [JsonPropertyName("account_id")]
        public string AccountId { get; set; }

        [JsonPropertyName("account_code")]
        public string AccountCode { get; set; }

        [JsonPropertyName("account_name")]
        public string AccountName { get; set; }

        [JsonPropertyName("address")]
        public string Address { get; set; }

        [JsonPropertyName("phone")]
        public string Phone { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("website")]
        public string Website { get; set; }

        [JsonPropertyName("birthday")]
        public string Birthday { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("relation_id")]
        public string RelationId { get; set; }

        [JsonPropertyName("revenue")]
        public string Revenue { get; set; }

        [JsonPropertyName("account_type")]
        public string AccountType { get; set; }

        [JsonPropertyName("created_at")]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("account_source")]
        public string AccountSource { get; set; }

        [JsonPropertyName("account_manager")]
        public string AccountManager { get; set; }

        [JsonPropertyName("province_name")]
        public string ProvinceName { get; set; }

        [JsonPropertyName("district_name")]
        public string DistrictName { get; set; }

        [JsonPropertyName("country_name")]
        public string CountryName { get; set; }

        [JsonPropertyName("industry_name")]
        public string IndustryName { get; set; }
    }

    public class ContactDetail
    {
        [JsonPropertyName("contact_id")]
        public string ContactId { get; set; }

        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }

        [JsonPropertyName("last_name")]
        public string LastName { get; set; }

        [JsonPropertyName("phone_mobile")]
        public string PhoneMobile { get; set; }

        [JsonPropertyName("phone_home")]
        public string PhoneHome { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }
    }
    public class InfoAllAccount
    {
        public Account account { get; set; }
        public List<Order>? order { get; set; }
    }
}
