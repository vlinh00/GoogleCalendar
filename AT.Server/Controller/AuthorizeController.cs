using AT.Share.Model;
using AT.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AT.Server.Data;
using AT.Server.Services.User;
using Microsoft.EntityFrameworkCore;

namespace AT_ORDER.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthorizeController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly IUserService _userService;

        public AuthorizeController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext context, IUserService userService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginParameters parameters)
        {
            var user = await _userManager.FindByNameAsync(parameters.UserName);
            if (user == null) return BadRequest("User does not exist");
            var singInResult = await _signInManager.CheckPasswordSignInAsync(user, parameters.Password, false);
            if (!singInResult.Succeeded) return BadRequest("Invalid password");

            await _signInManager.SignInAsync(user, parameters.RememberMe);

            return Ok();
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterParameters parameters)
        {
            var user = new ApplicationUser();
            user.UserName = parameters.UserName; 
            var result = await _userManager.CreateAsync(user, parameters.Password);
            if (!result.Succeeded) return BadRequest(result.Errors.FirstOrDefault()?.Description);

            if (result.Succeeded)
            {
                var staff = new Staff
                {
                    UserId = user.Id.ToString(),
                    GroupUserId = parameters.GroupUserId,
                    DepartmentId = parameters.DepartmentId
                };
                if (parameters.FullName != null)
                {
                    staff.Name = parameters.FullName;
                }
                else
                {
                    staff.Name = user.NormalizedUserName;
                }    
                _context.Staffs.Add(staff);
                await _context.SaveChangesAsync();
            }
            

            return await Login(new LoginParameters
            {
                UserName = parameters.UserName,
                Password = parameters.Password
            });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            //await DeleteTokenCalendar();
            return Ok();
        }

        [HttpGet]
        public UserInfo UserInfo()
        {
            //var user = await _userManager.GetUserAsync(HttpContext.User);
            return BuildUserInfo();
        }


        private UserInfo BuildUserInfo()
        {
            return new UserInfo
            {
                IsAuthenticated = User.Identity.IsAuthenticated,
                UserName = User.Identity.Name,
                ExposedClaims = User.Claims
                    //Optionally: filter the claims you want to expose to the client
                    //.Where(c => c.Type == "test-claim")
                    .ToDictionary(c => c.Type, c => c.Value)
            };
        }
        // Delete Token Calendar when logout
        private async Task DeleteTokenCalendar()
        {
            var userId = _userService.GetUserId();
            var staff = await _context.Staffs.FirstOrDefaultAsync(staff => staff.UserId == userId);
            if(staff.GroupUserId == 1)
            {
                var tokenPath = Path.Combine("token.json");
                if (Directory.Exists(tokenPath))
                {
                    Directory.Delete(tokenPath, true);
                }
            }    
        }
    }
}
