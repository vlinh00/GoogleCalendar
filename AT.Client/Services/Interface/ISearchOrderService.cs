using AT.Share.Model;

namespace AT.Client.Services.Interface
{
    public interface ISearchOrderService
    {
        Task<Order> GetOrderByCodeAsync(string orderCode);
        Task<Account> GetAccountByOrderCodeAsync(string orderCode);
    }
}
