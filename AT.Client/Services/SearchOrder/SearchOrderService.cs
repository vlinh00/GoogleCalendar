using AT.Client.Services.Interface;
using AT.Share.Model;
using System.Net.Http.Json;

namespace AT.Client.Services.SearchOrder
{
    public class SearchOrderService : ISearchOrderService
    {
        private readonly HttpClient _httpClient;
        public SearchOrderService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task <Order> GetOrderByCodeAsync(string orderCode)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<Order>($"api/SearchOrder/{orderCode}");
                return response;
            }
            catch (Exception)
            {

                return null;
            }     
            
        }
        public async Task<Account> GetAccountByOrderCodeAsync(string orderCode)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<Account>($"api/SearchOrder/account/{orderCode}");
                return response;
            }
            catch (Exception)
            {

                return null;
            }
        }
    }
}
