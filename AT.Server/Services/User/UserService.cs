using System.Security.Claims;
namespace AT.Server.Services.User
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string GetUserId()
        {
            //var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
            return userIdClaim != null ? userIdClaim.Value : string.Empty;
        }
    }
}
