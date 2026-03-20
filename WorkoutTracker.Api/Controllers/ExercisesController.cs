using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WorkoutTracker.Api.DTOs;
using WorkoutTracker.Api.Services;

namespace WorkoutTracker.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExercisesController : ControllerBase
    {
        private readonly IExerciseService _service;

        public ExercisesController(IExerciseService service)
        {
            _service = service;
        }

        // GET /api/exercises
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var exercises = await _service.GetAllAsync();
            return Ok(exercises);
        }

        // POST /api/exercises
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateExerciseDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var created = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetAll), new { id = created.Id }, created);
            }
            catch (System.ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
