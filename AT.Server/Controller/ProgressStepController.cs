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
    public class ProgressStepController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IDbModelService _dbModelService;
        public ProgressStepController(ApplicationDbContext context, IDbModelService dbModelService)
        {
            _context = context;
            _dbModelService = dbModelService;
        }
        // GET: api/<ProgressStepController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProgressStep>>> GetAllProgressStep()
        {
            return await _context.ProgressSteps.ToListAsync();
        }

        // GET api/<ProgressStepController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<List<ProgressStep>>> GetProgressStepByTaskId(int id)
        {
            var result = await _context.ProgressSteps.Where(pr => pr.TaskId == id).ToListAsync();

            if (result.Count <= 0)
            {
                return NotFound(); // Trả về NotFound nếu không tìm thấy
            }

            return Ok(result); // Trả về result tìm thấy
        }

        // POST api/<ProgressStepController>
        [HttpPost]
        public async Task<ActionResult<bool>> PostProgressStep(ProgressStep progressStep)
        {
            _context.ProgressSteps.Add(progressStep);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // PUT api/<ProgressStepController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProgressStep(int id, ProgressStep progressStep)
        {
            if (id != progressStep.Id)
            {
                return BadRequest();
            }

            _context.Entry(progressStep).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_dbModelService.ModelExists<ProgressStep>(id))
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

        // DELETE api/<ProgressStepController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
