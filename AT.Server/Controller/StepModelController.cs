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
    public class StepModelController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IDbModelService _dbModelService;
        public StepModelController(ApplicationDbContext context, IDbModelService dbModelService)
        {
            _context = context;
            _dbModelService = dbModelService;
        }
        // GET: api/<StepModelController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/<StepModelController>/5
        [HttpGet("{id}")]

        public async Task<ActionResult<List<StepModel>>> GetStepModelByOrderCode(string id)
        {
            var result = await _context.StepModels.Where(pr => pr.OrderCode == id).ToListAsync();
            return Ok(result);
        }

        // POST api/<StepModelController>
        [HttpPost]
        public async Task<ActionResult<bool>> PostStepModel(StepModel stepModel)
        {
            _context.StepModels.Add(stepModel);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("list")]
        public async Task<ActionResult<bool>> PostListStepModel(List<StepModel> stepModels)
        {
            if (stepModels == null || stepModels.Count == 0)
            {
                return BadRequest("Danh sách không được trống.");
            }
            _context.StepModels.AddRange(stepModels);
            await _context.SaveChangesAsync();

            return Ok(true);
        }

        // PUT api/<StepModelController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStepModel(int id, StepModel stepModel)
        {
            if (id != stepModel.Id)
            {
                return BadRequest();
            }

            _context.Entry(stepModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_dbModelService.ModelExists<StepModel>(id))
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

        // DELETE api/<StepModelController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
