using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WorkoutTracker.Api.DTOs;
using WorkoutTracker.Api.Services;

namespace WorkoutTracker.Api.Pages.Sets
{
    public class EditSetModel : PageModel
    {
        private readonly IWorkoutSetService _setService;
        private readonly IExerciseService _exerciseService;

        public EditSetModel(IWorkoutSetService setService, IExerciseService exerciseService)
        {
            _setService = setService;
            _exerciseService = exerciseService;
        }

        [BindProperty(SupportsGet = true)]
        public int SetId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int SessionId { get; set; }

        [BindProperty]
        public CreateWorkoutSetDto Input { get; set; }

        public IEnumerable<ExerciseDto> Exercises { get; set; }
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Exercises = await _exerciseService.GetAllAsync();

            var set = await _setService.GetByIdAsync(SetId);
            if (set == null) return NotFound();

            // Pre-populate form with existing values
            Input = new CreateWorkoutSetDto
            {
                ExerciseId = set.ExerciseId,
                Weight = set.Weight,
                Reps = set.Reps,
                SetNumber = set.SetNumber
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Exercises = await _exerciseService.GetAllAsync();

            if (!ModelState.IsValid)
                return Page();

            try
            {
                await _setService.UpdateSetAsync(SetId, Input);
                return RedirectToPage("/Sessions/Details", new { sessionId = SessionId });
            }
            catch (Exception ex) when (ex is ArgumentException || ex is KeyNotFoundException)
            {
                ErrorMessage = ex.Message;
                return Page();
            }
        }
    }
}
