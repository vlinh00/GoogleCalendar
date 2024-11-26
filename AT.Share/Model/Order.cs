using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AT.Share.Model
{
    public class Order
    {
        [JsonPropertyName("order_id")]
        public string OrderId { get; set; }

        [JsonPropertyName("order_code")]
        public string OrderCode { get; set; }

        [JsonPropertyName("order_type")]
        public string OrderType { get; set; }

        [JsonPropertyName("amount")]
        public string Amount { get; set; }

        [JsonPropertyName("f_amount")]
        public string FAmount { get; set; }

        [JsonPropertyName("discount")]
        public string Discount { get; set; }

        [JsonPropertyName("discount_amount")]
        public string DiscountAmount { get; set; }

        [JsonPropertyName("vat_amount")]
        public string VatAmount { get; set; }

        [JsonPropertyName("account_info")]
        public AccountInfo AccountInfo { get; set; }

        [JsonPropertyName("order_date")]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime OrderDate { get; set; }

        [JsonPropertyName("created_at")]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime UpdatedAt { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("assigned_user")]
        public string AssignedUser { get; set; }

        [JsonPropertyName("assigned_name")]
        public string AssignedName { get; set; }

        [JsonPropertyName("payment_status")]
        public string PaymentStatus { get; set; }

        [JsonPropertyName("lading_code")]
        public string LadingCode { get; set; }

        [JsonPropertyName("lading_info")]

        public List<LadingInfo> LadingInfo { get; set; }
    }


    public class AccountInfo
    {
        [JsonPropertyName("account_id")]
        public string AccountId { get; set; }

        [JsonPropertyName("account_code")]
        public string AccountCode { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("phone")]
        public string Phone { get; set; }

        [JsonPropertyName("address")]
        public string Address { get; set; }
    }

    public class LadingInfo
    {
        [JsonPropertyName("status")]
        public Status Status { get; set; }

        [JsonPropertyName("tracking_url")]
        public string TrackingUrl { get; set; }
    }

    public class Status
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
