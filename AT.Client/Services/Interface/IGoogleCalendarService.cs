using AT.Share.Model;
using Google.Apis.Calendar.v3.Data;

namespace AT.Client.Services.Interface
{
    public interface IGoogleCalendarService
    {
        Task<Events> GetAllEventsAsync();
        Task<Event> GetEventByIdAsync(string orderCode);
        Task<List<Events>> GetListAllEventsAsync();
        Task <List<DescriptionUpdate>> GetDescriptionUpdateAsync(string eventId); 
    }
}
