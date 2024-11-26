using AT.Share.Model;
using Google.Apis.Calendar.v3.Data;
using Blazored.Toast.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.Extensions.Logging;
using Google.Apis.Upload;
using System.Collections.Immutable;
using AT.Client.Services.SearchOrder;
using Blazorise;
using System;
using System.Reflection;


namespace AT.Client.Pages
{
    public partial class SearchOrder
    {
        private string orderCode;
        //private List<Events> listEvents = new List<Events>();
        //private Events events = new Events();
        private Event @event = new Event();
        private Account account = new Account();
        //private List<ProgressUpdate> progressUpdate = new List<ProgressUpdate>();
        //private List<StepModel> stepModels = new List<StepModel>();
        private List<DescriptionUpdate> descriptionUpdates = new List<DescriptionUpdate>();
        private List<DescriptionUpdate> descriptions = new List<DescriptionUpdate>();
        private DateTime dateStart;
        private DateTime dueDate;
        private string dateTwoPart;
        private string dateQC;
        private DateTime currentDate;
        private string assign;
        private List<string> progress = new List<string>();
        private List<string> timeLine = new List<string>();
        private List<string> description = new List<string>();
        private bool isCompleteTwoPart = false;
        private bool isCompleteQC = false;
        private bool isCompleteDone = false;
        private bool isCompletedNow = false;
        private int isOverTime = -1;
        private bool isLoading = false;
        private float widthStart_TwoPart;
        private float widthTwoPart_QC;
        private float widthQC_due;

        protected override async Task OnInitializedAsync()
        {

        }
        private async Task LoadEventById(string orderCode)
        {
            @event = await GoogleCalendarService.GetEventByIdAsync(orderCode);
        }

        private async Task LoadDescription(string eventId)
        {
            descriptionUpdates = await GoogleCalendarService.GetDescriptionUpdateAsync(eventId);
        }


        private async Task LoadAccountByOrderCode(string orderCode)
        {
            account = await SearchOrderService.GetAccountByOrderCodeAsync(orderCode);
        }

        private async void btnSearchOrder()
        {
            descriptionUpdates = new List<DescriptionUpdate>();
            descriptions = new List<DescriptionUpdate>();
            timeLine = new List<string>();
            description = new List<string>();
            account = new Account();
            @event = new Event();
            orderCode = orderCode.ToUpper();
            isCompleteTwoPart = false;
            isCompleteQC = false;
            isCompleteDone = false;
            isLoading = true;

            if (string.IsNullOrWhiteSpace(orderCode))
            {
                ToastService.ShowError("Vui lòng nhập mã đơn hàng");
                return;
            }
            if (orderCode.Length < 6)
            {
                ToastService.ShowError("Mã đơn hàng phải có ít nhất 6 ký tự");
                return;
            }

            await LoadEventById(orderCode);

            if (@event != null && !string.IsNullOrEmpty(@event?.Id))
            {
                assign = @event.Organizer.DisplayName.Split("-")[1].Trim();

                GetDateTime(@event);
                GetDateTwoPart(@event);
                GetDateQC(@event);
                GetQCProgress();
                DateTime twoPartDate = DateTime.ParseExact(dateTwoPart, "dd/MM/yyyy", null);
                DateTime qcDate = DateTime.ParseExact(dateQC, "dd/MM/yyyy", null);
                widthStart_TwoPart = GetCurrentProgress(dateStart, twoPartDate);
                widthTwoPart_QC = GetCurrentProgress(twoPartDate, qcDate);
                widthQC_due = GetCurrentProgress(qcDate, dueDate);
                GetTimeLine(@event);
                await LoadDescription(@event.Id);
                GetProgress();
                HandleCompleted();
                await LoadAccountByOrderCode(orderCode);
                if (account?.Info == null) ToastService.ShowError($"Không tim thấy thông tin khách hàng có mã đơn \"{orderCode}\"");
            }
            else
            {
                ToastService.ShowError($"Không tim thấy đơn hàng có mã \"{orderCode}\"");
            }
            isLoading = false;
            StateHasChanged();
        }

        private void GetDateTime(Event @event)
        {
            if (@event.Start.DateTime != null)
                dateStart = @event.Start.DateTime.Value;
            else if (@event.Start.Date != null)
                dateStart = DateTime.Parse(@event.Start.Date);
            else dateStart = DateTime.MinValue;

            if (@event.End.DateTime != null)
                dueDate = @event.End.DateTime.Value;
            else if (@event.End.Date != null)
                dueDate = DateTime.Parse(@event.End.Date).AddDays(-1);
            else dueDate = DateTime.MinValue;
            //int countDay = (dueDate - dateStart).Days * 2 / 3;
            //dateTwoPart = dateStart.AddDays(countDay);
        }

        private void GetDateTwoPart(Event @event)
        {
            if (@event.Description != null)
            {
                string[] parts = @event.Description.Split("\n");
                for (int i = 0; i < parts.Length; i++)
                {
                    parts[i] = parts[i].Replace("-", "").Trim();
                    if (parts[i].Contains("2/3"))
                    {
                        dateTwoPart = parts[i].Substring(0, parts[i].IndexOf(":"));
                        break;
                    }

                }
            }
        }
        private void GetDateQC(Event @event)
        {
            if (@event.Description != null)
            {
                string[] parts = @event.Description.Split("\n");
                for (int i = 0; i < parts.Length; i++)
                {
                    parts[i] = parts[i].Replace("-", "").Trim();
                    if (parts[i].ToLowerInvariant().Contains("qc".ToLowerInvariant()))
                    {
                        dateQC = parts[i].Substring(0, parts[i].IndexOf(":"));
                        break;
                    }

                }
            }
        }
        private float GetCurrentProgress(DateTime startDate, DateTime dueDate)
        {
            currentDate = DateTime.Now;
            if (currentDate.Date > startDate.Date && currentDate.Date < dueDate.Date)
            {
                int totalDuration = (dueDate.Date - startDate.Date).Days;
                int currentDuration = (currentDate.Date - startDate.Date).Days;
                float result = (float)currentDuration / totalDuration;

                // Đảm bảo giá trị tối thiểu là 0.12
                return result < 0.12f ? 0.12f : result;
            }
            else
            {
                return 1;
            }

        }

        private float GetQCProgress()
        {
            DateTime qcDate = DateTime.ParseExact(dateQC, "dd/MM/yyyy", null);
            DateTime twoPartDate = DateTime.ParseExact(dateTwoPart, "dd/MM/yyyy", null);

            if (qcDate.Date > twoPartDate.Date && qcDate < dueDate.Date)
            {
                int totalDuration = (dueDate - twoPartDate).Days;
                int qcDuration = (qcDate.Date - twoPartDate.Date).Days;
                float result = (float)qcDuration / totalDuration;

                // Đảm bảo giá trị tối thiểu là 0.12
                return result < 0.2f ? 0.2f : result;
            }
            else
            {
                return 1;
            }

        }

        private void GetProgress()
        {
            if (descriptionUpdates.Count > 0)
            {
                descriptions.Clear();
                int index = 10;
                for (int i = 0; i < descriptionUpdates.Count; i++)
                {
                    DateTime dateTime = descriptionUpdates[i].Timestamp ?? DateTime.UtcNow;

                    string[] parts = descriptionUpdates[i].Description.Split("\n");
                    if (i == 0)
                    {
                        for (int j = 0; j < parts.Length; j++)
                        {
                            if (parts[j].Trim().ToLowerInvariant() == "timeline".ToLowerInvariant()) index = j;
                            if (j > index)
                            {
                                parts[j] = parts[j].Replace("-", "").Trim();
                                if (parts[j].IndexOf("[v]", StringComparison.OrdinalIgnoreCase) >= 0)
                                {
                                    var temp = parts[j].Replace("[v]", "").Replace("[V]", "").Trim();
                                    var update = new DescriptionUpdate(temp, dateTime);
                                    descriptions.Add(update);
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int j = descriptions.Count + 1 + index; j < parts.Length; j++)
                        {

                            parts[j] = parts[j].Replace("-", "").Trim();
                            if (parts[j].IndexOf("[v]", StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                var temp = parts[j].Replace("[v]", "").Replace("[V]", "").Trim();
                                var update = new DescriptionUpdate(temp, dateTime);
                                descriptions.Add(update);

                            }
                        }
                    }

                    if (i == descriptionUpdates.Count - 1)
                    {
                        for (int j = descriptions.Count + index; j > index; j--)
                        {
                            parts[j] = parts[j].Replace("-", "").Trim();
                            if (parts[j].IndexOf("[v]", StringComparison.OrdinalIgnoreCase) < 0)
                            {
                                descriptions.RemoveAt(j - 1 - index);
                            }
                        }
                    }
                }
            }
        }


        private void GetTimeLine(Event @event)
        {
            int index = 10;
            if (@event.Description != null)
            {
                string[] eventParts = @event.Description.Split("\n");

                for (int i = 0; i < eventParts.Length; i++)
                {
                    if (eventParts[i].Trim().ToLowerInvariant() == "timeline".ToLowerInvariant()) index = i;
                    if (i > index)
                    {
                        eventParts[i] = eventParts[i].Replace("-", "").Trim();
                        string[] timeParts = eventParts[i].Split(":");
                        timeLine.Add(timeParts[0].Trim());
                        description.Add(timeParts[1].Trim());
                    }
                }

            }
        }

        // Xu ly trang thai complete
        private void HandleCompleted()
        {

            for (int k = 0; k < timeLine.Count; k++)
            {
                DateTime time = DateTime.ParseExact(timeLine[k], "dd/MM/yyyy", null);

                if (time.Date.AddDays(1) >= currentDate.Date)
                {
                    if (description[k].IndexOf("[v]", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        isCompletedNow = true;
                        break;
                    }
                    else
                    {
                        isCompletedNow = false;
                        break;
                    }
                }
            }

            DateTime twoPartDate = DateTime.ParseExact(dateTwoPart, "dd/MM/yyyy", null);
            for (int i = timeLine.Count - 1; i >= 0; i--)
            {
                DateTime time = DateTime.ParseExact(timeLine[i], "dd/MM/yyyy", null);
                DateTime qcDate = DateTime.ParseExact(dateQC, "dd/MM/yyyy", null);
                if (isCompleteDone == true && isCompleteQC == true && isCompleteTwoPart == true) break;
                if (description[timeLine.Count - 1].IndexOf("[v]", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    isCompleteTwoPart = true;
                    isCompleteQC = true;
                    isCompleteDone = true;
                    break;
                }
                if (time.Date.AddDays(1) >= dueDate.Date)
                {
                    if (description[i].IndexOf("[v]", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        isCompleteTwoPart = true;
                        isCompleteQC = true;
                        isCompleteDone = true;
                        break;
                    }
                }
                else if (time.Date.AddDays(1) >= qcDate.Date)
                {
                    if (description[i].IndexOf("[v]", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        isCompleteTwoPart = true;
                        isCompleteQC = true;
                        isCompleteDone = false;
                        break;
                    }
                }
                else if (time.Date.AddDays(1) >= twoPartDate.Date)
                {
                    if (description[i].IndexOf("[v]", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        isCompleteTwoPart = true;
                        isCompleteQC = false;
                        isCompleteDone = false;
                        break;
                    }
                }
                else if (time.Date < twoPartDate.Date)
                {
                    isCompleteTwoPart = false;
                    isCompleteQC = false;
                    isCompleteDone = false;
                    break;
                }
            }

            if (DateTime.Now.Date.AddDays(1) >= twoPartDate.Date)
            {
                if (isCompleteTwoPart == true)
                {
                    isOverTime = 0;
                }
                else
                {
                    isOverTime = 1;
                }
            }
            else
                isOverTime = -1;
        }
    }
}
