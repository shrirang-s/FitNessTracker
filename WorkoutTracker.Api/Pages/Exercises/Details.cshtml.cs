using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WorkoutTracker.Api.DTOs;
using WorkoutTracker.Api.Infrastructure;
using WorkoutTracker.Api.Services;

namespace WorkoutTracker.Api.Pages.Exercises
{
    public class ExerciseDetailsModel : PageModel
    {
        private readonly IExerciseService _service;

        public ExerciseDetailsModel(IExerciseService service)
        {
            _service = service;
        }

        public ExerciseDto Exercise { get; set; }
        public ExerciseInfo ExerciseInfo { get; set; }       // description + tips (may be null)
        public MuscleGroupInfo MuscleGroup { get; set; }     // diagram + muscles (may be null)

        public async Task<IActionResult> OnGetAsync(int exerciseId)
        {
            Exercise = await _service.GetByIdAsync(exerciseId);
            if (Exercise == null) return NotFound();

            // Look up static info — gracefully null if exercise isn't in the dictionary
            ExerciseInfo = ExerciseInfoProvider.GetExerciseInfo(Exercise.Name);
            MuscleGroup = ExerciseInfoProvider.GetMuscleGroupInfo(Exercise.MuscleGroup);

            return Page();
        }
    }
}
