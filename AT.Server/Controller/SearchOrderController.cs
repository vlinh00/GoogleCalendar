using AT.Client;
using AT.Server.Data;
using AT.Share.Model;
using Blazorise;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Text.Json;
using static NuGet.Packaging.PackagingConstants;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AT.Server.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchOrderController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        string ApiKey = "S4AwIQJXCnwjdsFYNkdmhjZopBubDQ";
        public SearchOrderController(ApplicationDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        private async Task<string> GetOrderByCodeDetails(string orderCode)
        {
            try
            {
                var apiUrlOrder = $"https://atpro.getflycrm.com/api/v3/orders/{orderCode}";

                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Add("X-API-KEY", ApiKey); // Thêm header API-Key vào yêu cầu

                var response = await client.GetAsync(apiUrlOrder);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();

                    // Phân tích cú pháp chuỗi JSON
                    using var jsonDoc = JsonDocument.Parse(jsonResponse);

                    // Truy xuất đến phần tử "order_info" và sau đó đến "account_code"
                    if (jsonDoc.RootElement.TryGetProperty("order_info", out JsonElement orderInfoElement) &&
                        orderInfoElement.TryGetProperty("account_code", out JsonElement accountCodeElement))
                    {
                        // Lấy giá trị account_code dưới dạng chuỗi
                        return accountCodeElement.GetString();
                    }
                    return null;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
                throw;
            }
            
        }
        // Get Account info
        private async Task<Account> GetAccountByIdDetail(string accountId)
        {
            try
            {
                var apiUrlAccount = $"https://atpro.getflycrm.com/api/v3/account?account_code=" + accountId;

                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Add("X-API-KEY", ApiKey); // Thêm header API-Key vào yêu cầu

                var response = await client.GetAsync(apiUrlAccount);

                if (response.IsSuccessStatusCode)
                {
                    // Đọc dữ liệu từ phản hồi
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    // Chuyển đổi chuỗi JSON thành accountResponse
                    var accountResponse = JsonSerializer.Deserialize<Account>(jsonResponse, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true // Để không phân biệt chữ hoa chữ thường
                    });

                    if (accountResponse != null)
                    {
                        return accountResponse;
                    }
                    return null;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
                throw;
            }
            
        }

        // GET: api/<SearchOrderController>
        [HttpGet("account/{id}")]
        public async Task<ActionResult<Account>> GetAccountById(string id)
        {
            var accountCode = await GetOrderByCodeDetails(id);
            if (accountCode == null) return Ok(new Account());

            var result = await GetAccountByIdDetail(accountCode);

            return Ok(result);
        }


        // GET api/<SearchOrderController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrderByCode(string id)
        {
            //var result = await GetOrderByCodeDetails(id);

            //if (result == null)
            //{
            //    return NotFound($"No orders found with OrderId: {id}");
            //}

            //return Ok(result);
            return Ok();
        }
        // POST api/<SearchOrderController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<SearchOrderController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<SearchOrderController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
