using AT.Server.Data;
using AT.Share.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Text.Json;
namespace AT.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchAccountController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        string ApiKey = "S4AwIQJXCnwjdsFYNkdmhjZopBubDQ";
        public SearchAccountController(ApplicationDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory; ;
        }
        private async Task<object> GetInfoDetails(string accountID)
        {
            var apiUrlAccount = $"https://atpro.getflycrm.com/api/v3/account?account_code="+ accountID; // Thay đổi thành API endpoint của bạn
            var apiUrlOrder = $"https://atpro.getflycrm.com/api/v3/orders?order_type=2"; // Thay đổi thành API endpoint của bạn

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("X-API-KEY", ApiKey); // Thêm header API-Key vào yêu cầu

            var responseOrder = await client.GetAsync(apiUrlOrder);
            var responseAccount = await client.GetAsync(apiUrlAccount);

            if (responseAccount.IsSuccessStatusCode && responseOrder.IsSuccessStatusCode)
            {
                // Đọc dữ liệu từ phản hồi
                var jsonResponseOrder = await responseOrder.Content.ReadAsStringAsync();
                var jsonReponseAccount = await responseAccount.Content.ReadAsStringAsync();
                var account = JsonSerializer.Deserialize<Account>(jsonReponseAccount, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                // Chuyển đổi chuỗi JSON thành OrderResponse
                var orderResponse = JsonSerializer.Deserialize<OrderResponse>(jsonResponseOrder, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true // Để không phân biệt chữ hoa chữ thường
                });
                var result = orderResponse.Records.Where(order => order.AccountInfo.AccountCode == accountID).ToList();
                var kq = new InfoAllAccount { account = account, order = result };
                return kq;
            }
            else
            {
                return null;
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Object>> GetOrder(string id)
        {
            return await GetInfoDetails(id.ToString());
        }
    }

}
