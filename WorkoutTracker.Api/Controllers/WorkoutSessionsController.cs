using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WorkoutTracker.Api.DTOs;
using WorkoutTracker.Api.Services;

namespace WorkoutTracker.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkoutSessionsController : ControllerBase
    {
        private readonly IWorkoutSessionService _service;

        public WorkoutSessionsController(IWorkoutSessionService service)
        {
            _service = service;
        }

        // GET /api/workoutsessions
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var sessions = await _service.GetAllAsync();
            return Ok(sessions);
        }

        // GET /api/workoutsessions/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var session = await _service.GetByIdAsync(id);
            if (session == null)
                return NotFound(new { error = $"Session {id} not found." });

            return Ok(session);
        }

        // POST /api/workoutsessions
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateWorkoutSessionDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
    }
}
