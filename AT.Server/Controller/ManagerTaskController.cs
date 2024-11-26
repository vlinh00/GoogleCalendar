using AT.Client.Services.Interface;
using AT.Server.Data;
using AT.Server.Services.DbModel;
//using AT.Server.Services.ManagerTaskModel;
using AT.Server.Services.User;
using AT.Share.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AT.Server.Controller
{
    [Route("api/[controller]")]
    [ApiController]

    public class ManagerTaskController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserService _userService;
        private readonly IDbModelService _dbModelService;
        //private readonly ITaskModelService _taskModelService;
        public ManagerTaskController(ApplicationDbContext context, IUserService userService, IDbModelService dbModelService)
        {
            _context = context;
            _userService = userService;
            _dbModelService = dbModelService;
        }

        // GET: api/<ManagerTaskController>
        [HttpGet("all-task")]
        public async Task<ActionResult<IEnumerable<TaskModel>>> GetAllTask()
        {
            return await _context.Tasks.ToListAsync();
        }

        // GET api/<ManagerTaskController>
        [HttpGet("all-task-by-UserId")]

        public async Task<ActionResult<IEnumerable<TaskModel>>> GetAllTaskByUserId()
        {
            string userId = _userService.GetUserId();
            return await _context.Tasks.Where(task => task.StaffId == userId).ToListAsync();
        }

        // GET api/<ManagerTaskController>/5
        [HttpGet("{id}")]

        public async Task<ActionResult<TaskModel>> GetTaskById(int id)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);

            if (task == null)
            {
                return NotFound(); // Trả về NotFound nếu không tìm thấy
            }

            return Ok(task); // Trả về task tìm thấy
        }

        // POST api/<ManagerTaskController>
        [HttpPost]
        public async Task<ActionResult<bool>> PostTaskModel(TaskModel taskModel)
        {
            //bool result = await _taskModelService.AddTaskModel(taskModel);
            //if(result) return Ok();
            //else return BadRequest();
            return Ok();
        }

        // PUT api/<ManagerTaskController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTaskModel(int id, TaskModel taskModel)
        {
            try
            {
                //bool result = await _taskModelService.UpdateTaskModel(id, taskModel);
                //if (result) return NotFound();
                //else return BadRequest();
                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {

                if (!_dbModelService.ModelExists<TaskModel>(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

        }

        // DELETE api/<ManagerTaskController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTaskModel(int id)
        {
            var taskModel = await _context.Tasks.FindAsync(id);
            if (taskModel == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(taskModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
