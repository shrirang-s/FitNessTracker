using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WorkoutTracker.Api.DTOs;
using WorkoutTracker.Api.Services;

namespace WorkoutTracker.Api.Pages.Exercises
{
    public class ExerciseHistoryModel : PageModel
    {
        private readonly IWorkoutSetService _setService;
        private readonly IExerciseService _exerciseService;

        public ExerciseHistoryModel(IWorkoutSetService setService, IExerciseService exerciseService)
        {
            _setService = setService;
            _exerciseService = exerciseService;
        }

        public string ExerciseName { get; set; }
        public IEnumerable<WorkoutSetDto> Sets { get; set; } = new List<WorkoutSetDto>();

        public async Task<IActionResult> OnGetAsync(int exerciseId)
        {
            var exercise = await _exerciseService.GetAllAsync();
            var match = exercise.FirstOrDefault(e => e.Id == exerciseId);
            if (match == null) return NotFound();

            ExerciseName = match.Name;
            Sets = await _setService.GetExerciseHistoryAsync(exerciseId);
            return Page();
        }
    }
}
