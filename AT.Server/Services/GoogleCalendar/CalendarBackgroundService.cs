using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using AT.Server.Services.GoogleCalendar;
using AT.Share.Model;
using Azure.Core;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class CalendarBackgroundService : BackgroundService
{
    private readonly IWebHostEnvironment _env;
    private CalendarService _calendarService;
    private readonly ILogger<CalendarBackgroundService> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private static List<Events> _cachedEvents = new List<Events>(); // Bộ nhớ lưu dữ liệu trong RAM

    public CalendarBackgroundService(ILogger<CalendarBackgroundService> logger, IWebHostEnvironment env, IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _env = env;
        InitializeCalendarService();
        _serviceScopeFactory = serviceScopeFactory;
    }
    private void InitializeCalendarService()
    {
        var credentialsPath = Path.Combine(_env.ContentRootPath, "credentials.json");
        if (!File.Exists(credentialsPath))
        {
            _logger.LogError("Credentials file not found at path: {credentialsPath}", credentialsPath);
            return;
        }
        var tokenPath = Path.Combine("token.json");

        using (var stream = new FileStream(credentialsPath, FileMode.Open, FileAccess.Read))
        {
            var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                GoogleClientSecrets.Load(stream).Secrets,
                new[] { CalendarService.Scope.CalendarReadonly },
                "user",
                CancellationToken.None,
                new FileDataStore(tokenPath, true)).Result;
            if (credential == null) { return; }
            _calendarService = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Google Calendar",
            });
        }
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // Tạo một scope mới để sử dụng các dịch vụ Scoped
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    // Lấy dịch vụ CalendarService từ scope mới
                    var calendarService = scope.ServiceProvider.GetRequiredService<CalendarService>();

                    // Gọi API lấy dữ liệu lịch
                    await FetchCalendarDataAsync();

                    _logger.LogInformation("Lịch đã được cập nhật vào bộ nhớ RAM tại: {time}", DateTimeOffset.Now);
                }

                // Gọi API lấy dữ liệu lịch
                //await FetchCalendarDataAsync();
                //_logger.LogInformation("Lịch đã được cập nhật vào bộ nhớ RAM tại: {time}", DateTimeOffset.Now);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã xảy ra lỗi khi lấy dữ liệu lịch");
            }

            // Thiết lập thời gian chờ trước khi gọi lại API, 1 phút
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }

    private static Dictionary<string, EventHistory> _eventHistories = new Dictionary<string, EventHistory>();
    private async Task FetchCalendarDataAsync()
    {
        List<Events> lstEvents = new List<Events> { };
        var calendarListRequest = _calendarService.CalendarList.List();
        var calendars = calendarListRequest.Execute();

        for (int i = 0; i < calendars.Items.Count; i++)
        {
            if (calendars.Items[i] != null)
            {
                var request = _calendarService.Events.List(calendars.Items[i].Id);
                request.TimeMin = DateTime.UtcNow.AddDays(-15);
                request.ShowDeleted = false;
                request.SingleEvents = true;
                request.MaxResults = 40;
                request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;
                request.Q = "TIMELINE"; // Filter by keyword in Summary, Description, or Location

                var events = request.Execute();

                //If events contain the keyword and match the given id, return it
                if (events.Summary.Contains("TIMELINE"))
                {
                    lstEvents.Add(events);
                    foreach (var ev in events.Items)
                    {
                        if (!_eventHistories.ContainsKey(ev.Id))
                        {
                            // Nếu sự kiện chưa có trong danh sách lịch sử, tạo lịch sử mới
                            DateTime updatedTime = ev.Updated ?? DateTime.UtcNow;
                            var history = new EventHistory(ev.Id, ev.Description, updatedTime);
                            //history.AddUpdateTimestamp(ev.Description);
                            _eventHistories[ev.Id] = history;
                        }
                        else
                        {
                            // Nếu sự kiện đã tồn tại, kiểm tra xem có cần cập nhật không
                            DateTime updatedTime = ev.Updated ?? DateTime.UtcNow;
                            var history = _eventHistories[ev.Id];
                            if (EventNeedsUpdate(history, ev)) // Phương thức kiểm tra sự kiện đã thay đổi chưa
                            {
                                _eventHistories[ev.Id].AddUpdateTimestamp(ev.Description, updatedTime);
                            }
                        }
                    }
                }

                CleanOldEventHistories();

            }
        }
        if(lstEvents.Count > 0 && !AreEventListsEqual(lstEvents, _cachedEvents))
        {
            _cachedEvents = lstEvents;
        }    
        //_cachedEvents = lstEvents.Count > 0 ? lstEvents : new List<Events>();
    }

    private bool AreEventListsEqual(List<Events> list1, List<Events> list2)
    {
        if (list1.Count != list2.Count) return false;

        for (int i = 0; i < list1.Count; i++)
        {
            if(list1[i].Updated != list2[i].Updated) return false;
            //if (!list1[i].Equals(list2[i])) return false;
        }
        return true;
    }

    // Phương thức kiểm tra xem một sự kiện có cần cập nhật không
    private bool EventNeedsUpdate(EventHistory history, Event ev)
    {
        // So sánh thông tin sự kiện để xem có thay đổi gì không
        return history.UpdateHistory.Last().Description != ev.Description; // Ví dụ: so sánh dựa trên tóm tắt
    }

    private void CleanOldEventHistories()
    {
        var now = DateTime.UtcNow;

        foreach (var eventHistory in _eventHistories.ToList())
        {
            // Giả sử bạn có thông tin về thời gian cập nhật cuối cùng của sự kiện
            var lastUpdate = eventHistory.Value.UpdateHistory.LastOrDefault()?.Timestamp;

            // Nếu sự kiện không được cập nhật trong vòng 30 ngày
            if (lastUpdate.HasValue && now - lastUpdate.Value > TimeSpan.FromDays(30))
            {
                _eventHistories.Remove(eventHistory.Key);
            }
        }
    }
    public static List<Events> GetCachedEvents() => _cachedEvents;

    public static List<DescriptionUpdate> GetEventUpdateHistory(string eventId)
    {
        // Kiểm tra nếu _eventHistories có dữ liệu và eventId tồn tại trong đó
        if (_eventHistories != null && _eventHistories.ContainsKey(eventId))
        {
            return _eventHistories[eventId].UpdateHistory;
        }

        // Nếu không có dữ liệu, trả về một danh sách trống
        return new List<DescriptionUpdate>();
        //return _eventHistories.ContainsKey(eventId) ? _eventHistories[eventId].UpdateHistory : new List<DescriptionUpdate>();
    }
}
