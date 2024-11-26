using AT.Server.Data;
using AT.Server.Models;
using AT.Server.Services.DbModel;
using AT.Server.Services.User;
using AT.Share.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AT.Server.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagerUserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserService _userService;
        private readonly IDbModelService _dbModelService;
        public ManagerUserController(ApplicationDbContext context, IUserService userService, IDbModelService dbModelService)
        {
           _context = context;
            _userService = userService;
            _dbModelService = dbModelService;
        }
        // GET: api/<ManagerUserController>
        [HttpGet("all-user")]
        public async Task<ActionResult<IEnumerable<Staff>>> GetAllUser()
        {
            return await _context.Staffs.ToListAsync();
        }

        [HttpGet("all-groupuser")]
        public async Task<ActionResult<IEnumerable<GroupUser>>> GetAllGroupUser()
        {
            return await _context.GroupUsers.ToListAsync();
        }

        [HttpGet("all-groupuser-byId")]
        public async Task<ActionResult<IEnumerable<Staff>>> GetStaffById()
        {
            string userId = _userService.GetUserId();
            var result = await _context.Staffs.FirstOrDefaultAsync(staff => staff.UserId == userId);
            return Ok(result);
        }

        [HttpGet("all-user-info")]
        public async Task<ActionResult<IEnumerable<StaffInfo>>> GetAllUserInfo()
        {
            var result = await (from staff in _context.Staffs
                                               join department in _context.Departments
                                               on staff.DepartmentId equals department.Id
                                               join groupUser in _context.GroupUsers
                                               on staff.GroupUserId equals groupUser.Id
                                               select new StaffInfo
                                               {
                                                   staffs = staff,
                                                   DepartmentName = department.Name,
                                                   GroupUserName = groupUser.GroupName
                                               }).ToListAsync();
  
            return Ok(result);
        }

        [HttpGet("all-user-info-byId")]
        public async Task<ActionResult<IEnumerable<StaffInfo>>> GetAllUserInfoById()
        {
            string userId = _userService.GetUserId();
            var result = await (from staff in _context.Staffs
                                join department in _context.Departments
                                on staff.DepartmentId equals department.Id
                                join groupUser in _context.GroupUsers
                                on staff.GroupUserId equals groupUser.Id
                                where staff.UserId == userId
                                select new StaffInfo
                                {
                                    staffs = staff,
                                    DepartmentName = department.Name,
                                    GroupUserName = groupUser.GroupName
                                }).FirstOrDefaultAsync();

            return Ok(result);
        }


        // GET api/<ManagerUserController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ManagerUserController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ManagerUserController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserInfo(int id, Staff staff)
        {
            if (id != staff.Id)
            {
                return BadRequest();
            }

            _context.Entry(staff).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_dbModelService.ModelExists<Staff>(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE api/<ManagerUserController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserById(string id)
        {
            var staff = await _context.Staffs.FindAsync(id);
            if (staff == null)
            {
                return NotFound();
            }

            _context.Staffs.Remove(staff);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
