using System;
using System.Text.Json;
using System.Text.Json.Serialization;

public class CustomDateTimeConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var dateString = reader.GetString();

        // Thử phân tích định dạng "yyyy-MM-dd"
        if (DateTime.TryParse(dateString, out DateTime date))
        {
            return date;
        }

        // Thử phân tích định dạng "yyyy-MM-dd HH:mm:ss"
        if (DateTime.TryParseExact(dateString, "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out date))
        {
            return date;
        }

        // Nếu không thành công, ném ngoại lệ
        throw new JsonException($"Unable to convert \"{dateString}\" to DateTime.");
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        // Ghi DateTime về JSON với định dạng "yyyy-MM-dd HH:mm:ss"
        writer.WriteStringValue(value.ToString("yyyy-MM-dd HH:mm:ss"));
    }
}
