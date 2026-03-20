using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WorkoutTracker.Api.DTOs;
using WorkoutTracker.Api.Services;

namespace WorkoutTracker.Api.Controllers
{
    [ApiController]
    [Route("api")]
    public class WorkoutSetsController : ControllerBase
    {
        private readonly IWorkoutSetService _service;

        public WorkoutSetsController(IWorkoutSetService service)
        {
            _service = service;
        }

        // POST /api/workoutsessions/{sessionId}/sets
        [HttpPost("workoutsessions/{sessionId}/sets")]
        public async Task<IActionResult> AddSet(int sessionId, [FromBody] CreateWorkoutSetDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var created = await _service.AddSetToSessionAsync(sessionId, dto);
                return CreatedAtAction(nameof(GetExerciseHistory),
                    new { exerciseId = created.ExerciseId }, created);
            }
            catch (System.Collections.Generic.KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (System.ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // GET /api/exercises/{exerciseId}/history
        [HttpGet("exercises/{exerciseId}/history")]
        public async Task<IActionResult> GetExerciseHistory(int exerciseId)
        {
            var history = await _service.GetExerciseHistoryAsync(exerciseId);
            return Ok(history);
        }
    }
}
