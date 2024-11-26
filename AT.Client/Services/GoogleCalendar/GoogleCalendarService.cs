using AT.Client.Services.Interface;
using AT.Share.Model;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.Net.Http.Json;
using System.Text.Json;

namespace AT.Client.Services.GoogleCalendar
{

    public class GoogleCalendarService : IGoogleCalendarService
    {
        private readonly HttpClient _httpClient;
        public GoogleCalendarService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Events> GetAllEventsAsync()
        {
            return await _httpClient.GetFromJsonAsync<Events>("api/GoogleCalendar/upcoming-events");
        }

        public async Task<List<DescriptionUpdate>> GetDescriptionUpdateAsync(string eventId)
        {
            try
            {
                var result = await _httpClient.GetFromJsonAsync<List<DescriptionUpdate>>($"api/GoogleCalendar/descriptionUpdate/{eventId}", new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    IncludeFields = true
                });
                return result;
            }
            catch (HttpRequestException httpEx)
            {
                // Xử lý ngoại lệ cụ thể cho lỗi HTTP nếu cần
                Console.WriteLine($"HTTP Request error: {httpEx.Message}");
                return new List<DescriptionUpdate>();
            }
            catch (Exception ex)
            {
                // Log các lỗi khác nếu cần thiết
                Console.WriteLine($"An error occurred: {ex.Message}");
                return new List<DescriptionUpdate>();
            }

        }

        public async Task<Event> GetEventByIdAsync(string orderCode)
        {
            return await _httpClient.GetFromJsonAsync<Event>($"api/GoogleCalendar/{orderCode}");
        }

        public async Task<List<Events>> GetListAllEventsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Events>>("api/GoogleCalendar/all-events");
        }
    }
}
