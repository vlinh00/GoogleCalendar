using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using AT.Server.Services.User;
using NuGet.Protocol;
using Blazorise;
using AT.Server.Data;
using AT.Share.Model;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Google;
using Google.Apis.Requests;
using Microsoft.DotNet.MSIdentity.Shared;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AT.Server.Controller
{
    [Route("api/[controller]")]
    [ApiController]

    public class GoogleCalendarController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private CalendarService _calendarService;
        private readonly IUserService _userService;
        private readonly ApplicationDbContext _context;
        private readonly HttpClient _httpClient;
        //private readonly ITaskModelService _taskModelService;

        public GoogleCalendarController(IWebHostEnvironment env, IUserService userService, ApplicationDbContext applicationDbContext, HttpClient httpClient)
        {
            _env = env;
            _userService = userService;
            _context = applicationDbContext;
            InitializeCalendarService();
            _httpClient = httpClient;
        }

        private void InitializeCalendarService()
        {
            var credentialsPath = Path.Combine(_env.ContentRootPath, "credentials.json");
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

        // GET: api/<GoogleCalendarController>
        [HttpGet("upcoming-events")]
        public async Task<ActionResult<IEnumerable<Events>>> GetUpcomingEvents()
        {
            string userId = null;
            var calendarListRequest = _calendarService.CalendarList.List();
            var calendars = calendarListRequest.Execute();
            List<TaskModel> taskModels = new List<TaskModel>();
            Events events = new Events();
            userId = _userService.GetUserId();
            var staff = _context.Staffs.FirstOrDefault(staff => staff.UserId == userId);
            if (staff != null)
            {
                if (staff.GroupUserId > 1)
                {
                    var calendar = calendars.Items.Where(calendar => calendar.Summary == staff.SummaryName).FirstOrDefault();
                    if (staff.CalendarId == null)
                    {
                        staff.CalendarId = calendar.Id;
                        _context.Staffs.Update(staff);
                        await _context.SaveChangesAsync();
                    }
                    string calendarId = null;
                    if (calendar != null) calendarId = calendar.Id;
                    else calendarId = "primary";

                    var request = _calendarService.Events.List(calendarId);
                    request.TimeMin = DateTime.UtcNow;
                    request.ShowDeleted = false;
                    request.SingleEvents = true;
                    request.MaxResults = 10;
                    request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

                    events = request.Execute();

                    taskModels = await _context.Tasks.Where(task => task.StaffId == userId).ToListAsync();
                    try
                    {
                        foreach (var item in events.Items)
                        {
                            var parts = item.Summary.Split('-');
                            parts[0] = parts[0].Trim();
                            var taskModel = taskModels.FirstOrDefault(task => task.OrderCode == parts[0]);

                            var task = new TaskModel()
                            {
                                OrderCode = parts[0],
                                StaffId = userId,
                                StaffName = staff.Name,
                                DateFinish = taskModels[0].DateFinish,
                                EventId = item.Id
                            };
                            if (item.Start.DateTime != null)
                                task.DateStart = item.Start.DateTime.Value;
                            else if (item.Start.Date != null)
                                task.DateStart = DateTime.Parse(item.Start.Date);
                            else task.DateStart = DateTime.MinValue;

                            if (item.End.DateTime != null)
                                task.DueDate = item.End.DateTime.Value;
                            else if (item.End.Date != null)
                                task.DateStart = DateTime.Parse(item.End.Date);
                            else task.DueDate = DateTime.MinValue;

                            if (taskModel == null)
                            {
                                task.Status = "Not Started";
                                task.Progress = " ";
                                //await _taskModelService.AddTaskModel(task);
                            }
                            else
                            {
                                task.Id = taskModel.Id;
                                task.Status = taskModel.Status;
                                task.Progress = taskModel.Progress;
                                //if (IsTaskModelsDifferent(task, taskModel))
                                   //await _taskModelService.UpdateTaskModel(task.Id, task);
                            }
                        }
                    }
                    catch (Exception)
                    {

                        return Ok(events);
                    }

                    return Ok(events);
                }
                else
                {
                    var allEvents = new List<Events>();
                    foreach (var calendar in calendars.Items)
                    {
                        if (calendar != null)
                        {
                            var request = _calendarService.Events.List(calendar.Id);
                            request.TimeMin = DateTime.UtcNow;
                            request.ShowDeleted = false;
                            request.SingleEvents = true;
                            request.MaxResults = 10;
                            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

                            var events1 = request.Execute();
                            if (events1 != null)
                            {
                                allEvents.Add(events1);
                            }
                            //return Ok(events1);
                        }

                    }
                    return Ok(allEvents);
                    //return Ok(allEvents);
                    //return NotFound("No upcoming events found.");
                }
            }
            else
            {
                return NotFound("No upcoming events found.");
            }

            //var allEvents = new List<Event>();
        }


        // GET api/<GoogleCalendarController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Event>>> GetEventById(string id)
        {
            var events = CalendarBackgroundService.GetCachedEvents();
            if (events.Count > 0)
            {

                var @event = await Task.Run(() =>
                {
                    return events
                        .SelectMany(e => e.Items) // Truy xuất tất cả các phần tử từ danh sách `Items` bên trong
                        .FirstOrDefault(item => item.Summary?.Split("_")[0] == id);
                });

                if (@event != null)
                    return Ok(@event);
                return Ok(new Event());
            }
            else
                return Ok(new Event());

        }

        // API để lấy lịch sử cập nhật cho sự kiện
        [HttpGet("descriptionUpdate/{eventId}")]
        public async Task<ActionResult<IEnumerable<List<DescriptionUpdate>>>> GetDescriptionUpdateById(string eventId)
        {
            // Lấy lịch sử cập nhật từ backend
            var updateHistory = CalendarBackgroundService.GetEventUpdateHistory(eventId);

            if (updateHistory.Count == 0 || !updateHistory.Any())
            {
                return Ok(new List<DescriptionUpdate>());
            }

            // Serialize với JsonSerializerOptions để không phân biệt chữ hoa chữ thường
            var json = System.Text.Json.JsonSerializer.Serialize(updateHistory, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            });

            return Ok(updateHistory);
        }











        // POST api/<GoogleCalendarController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<GoogleCalendarController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCalendarEvent(string id, TaskModel task)
        {
            try
            {
                bool result = await UpdateEvent(id, task);
                if (!result)
                    return BadRequest();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }

            return NoContent();
        }

        private async Task<bool> UpdateEvent(string calendarId, TaskModel task)
        {
            try
            {
                Event existingEvent = new Event();
                existingEvent = await _calendarService.Events.Get(calendarId, task.EventId).ExecuteAsync();
                if (existingEvent == null) return false;

                if (existingEvent.Start.Date != null) existingEvent.Start.Date = task.DateStart.ToString();
                else if (existingEvent.Start.DateTime != null) existingEvent.Start.DateTime = task.DateStart;

                if (existingEvent.End.Date != null) existingEvent.End.Date = task.DateFinish.ToString();
                else if (existingEvent.End.DateTime != null) existingEvent.End.DateTime = task.DateFinish;

                existingEvent.Description += $"\n Tiến độ: {task.Progress}\n Trạng thái: {task.Status}";

                // Gọi phương thức Update để cập nhật sự kiện
                await _calendarService.Events.Update(existingEvent, calendarId, task.EventId).ExecuteAsync();
                return true;
            }
            catch (GoogleApiException ex)
            {
                Console.WriteLine($"Google API error: {ex.Message}\nStatus: {ex.HttpStatusCode}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
                return false;
            }
        }


        // DELETE api/<GoogleCalendarController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        private bool IsTaskModelsDifferent(TaskModel task, TaskModel taskModel)
        {
            var taskJson = JsonConvert.SerializeObject(task);
            var taskModelJson = JsonConvert.SerializeObject(taskModel);
            return taskJson != taskModelJson;
        }
    }
}
