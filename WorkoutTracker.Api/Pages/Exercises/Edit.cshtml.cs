using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WorkoutTracker.Api.DTOs;
using WorkoutTracker.Api.Infrastructure;
using WorkoutTracker.Api.Services;

namespace WorkoutTracker.Api.Pages.Exercises
{
    public class EditExerciseModel : PageModel
    {
        private readonly IExerciseService _service;

        public EditExerciseModel(IExerciseService service)
        {
            _service = service;
        }

        [BindProperty(SupportsGet = true)]
        public int ExerciseId { get; set; }

        [BindProperty]
        public CreateExerciseDto Input { get; set; }

        public IEnumerable<string> MuscleGroups { get; set; }
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            MuscleGroups = ExerciseInfoProvider.GetMuscleGroups();

            var exercise = await _service.GetByIdAsync(ExerciseId);
            if (exercise == null) return NotFound();

            // Pre-populate form with existing values
            Input = new CreateExerciseDto
            {
                Name = exercise.Name,
                MuscleGroup = exercise.MuscleGroup
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            MuscleGroups = ExerciseInfoProvider.GetMuscleGroups();

            if (!ModelState.IsValid)
                return Page();

            try
            {
                await _service.UpdateAsync(ExerciseId, Input);
                return RedirectToPage("/Exercises/Index");
            }
            catch (ArgumentException ex)
            {
                ErrorMessage = ex.Message;
                return Page();
            }
            catch (KeyNotFoundException ex)
            {
                ErrorMessage = ex.Message;
                return Page();
            }
        }
    }
}
