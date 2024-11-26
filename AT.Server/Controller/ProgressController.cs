using AT.Share.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AT.Server.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AT.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProgressController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProgressController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Progress
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProgressModel>>> GetProgresses()
        {
            // Lấy danh sách tiến độ
            var progresses = await _context.Progresses.ToListAsync();
            return Ok(progresses);
        }

        // GET: api/Progress/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProgressModel>> GetProgress(int id)
        {
            var progress = await _context.Progresses.FindAsync(id);

            if (progress == null)
            {
                return NotFound();
            }

            return Ok(progress);
        }

        // POST: api/Progress
        [HttpPost]
        public async Task<ActionResult<ProgressModel>> PostProgress(ProgressModel progress)
        {
            _context.Progresses.Add(progress);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProgress), new { id = progress.Id }, progress);
        }

        // PUT: api/Progress/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProgress(int id, ProgressModel progress)
        {
            if (id != progress.Id)
            {
                return BadRequest();
            }

            _context.Entry(progress).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProgressExists(id))
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

        // DELETE: api/Progress/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProgress(int id)
        {
            var progress = await _context.Progresses.FindAsync(id);
            if (progress == null)
            {
                return NotFound();
            }

            _context.Progresses.Remove(progress);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<ProjectTypeModel> GetProjectTypeByIdAsync(int projectTypeId)
        {
            return await _context.ProjectTypes
                .FirstOrDefaultAsync(pt => pt.Id == projectTypeId);
        }

        private bool ProgressExists(int id)
        {
            return _context.Progresses.Any(e => e.Id == id);
        }
    }
}
