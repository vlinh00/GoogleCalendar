using AT.Share.Model;
using AT.Server.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AT.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectTypeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProjectTypeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ProjectType
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectTypeModel>>> GetProjectTypes()
        {
            return await _context.ProjectTypes.ToListAsync();
        }

        // GET: api/ProjectType/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectTypeModel>> GetProjectType(int id)
        {
            var projectType = await _context.ProjectTypes.FindAsync(id);

            if (projectType == null)
            {
                return NotFound();
            }

            return projectType;
        }

        // POST: api/ProjectType
        [HttpPost]
        public async Task<ActionResult<ProjectTypeModel>> PostProjectType(ProjectTypeModel projectType)
        {
            _context.ProjectTypes.Add(projectType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProjectType", new { id = projectType.Id }, projectType);
        }

        // PUT: api/ProjectType/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProjectType(int id, ProjectTypeModel projectType)
        {
            if (id != projectType.Id)
            {
                return BadRequest();
            }

            _context.Entry(projectType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectTypeExists(id))
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

        // DELETE: api/ProjectType/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProjectType(int id)
        {
            var projectType = await _context.ProjectTypes.FindAsync(id);
            if (projectType == null)
            {
                return NotFound();
            }

            _context.ProjectTypes.Remove(projectType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProjectTypeExists(int id)
        {
            return _context.ProjectTypes.Any(e => e.Id == id);
        }
    }
}
