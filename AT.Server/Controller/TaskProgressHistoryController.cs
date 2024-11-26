using AT.Server.Data;
using AT.Share.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AT.Server.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskProgressHistoryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public TaskProgressHistoryController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: api/<TaskProgressHistoryController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<TaskProgressHistoryController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<List<TaskProgressHistory>>> GetTaskProgressById(int id)
        {
            var result = await _context.TaskProgressHistorys.Where(task => task.TaskId == id).ToListAsync();

            if (result.Count <= 0)
            {
                return NotFound(); // Trả về NotFound nếu không tìm thấy
            }

            return Ok(result); // Trả về result tìm thấy
        }

        // POST api/<TaskProgressHistoryController>
        [HttpPost]
        public async Task<ActionResult<bool>> PostTaskProgressHistory(TaskProgressHistory taskProgressHistory)
        {
            _context.TaskProgressHistorys.Add(taskProgressHistory);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // PUT api/<TaskProgressHistoryController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TaskProgressHistoryController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
