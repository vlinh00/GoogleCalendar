using AT.Server.Data;
using AT.Server.Services.DbModel;
using AT.Share.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AT.Server.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProgressUpdateController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IDbModelService _dbModelService;
        public ProgressUpdateController(ApplicationDbContext context, IDbModelService dbModelService)
        {
            _context = context;
            _dbModelService = dbModelService;
        }
        // GET: api/<ProgressUpdateController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ProgressUpdateController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<List<ProgressUpdate>>> GetProgressUpdateByOrderCode(string id)
        {
            var result = await _context.ProgressUpdates.Where(pr => pr.OrderCode == id).ToListAsync();

            return Ok(result);
        }
        // POST api/<ProgressUpdateController>
        [HttpPost]
        public async Task<ActionResult<bool>> PostProgress(ProgressUpdate progressUpdate)
        {
            _context.ProgressUpdates.Add(progressUpdate);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // PUT api/<ProgressUpdateController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProgress(int id, ProgressUpdate progressUpdate)
        {
            if (id != progressUpdate.Id)
            {
                return BadRequest();
            }

            _context.Entry(progressUpdate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_dbModelService.ModelExists<ProgressUpdate>(id))
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

        // DELETE api/<ProgressUpdateController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
