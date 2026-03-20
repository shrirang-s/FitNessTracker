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
    public class CreateExerciseModel : PageModel
    {
        private readonly IExerciseService _service;

        public CreateExerciseModel(IExerciseService service)
        {
            _service = service;
        }

        [BindProperty]
        public CreateExerciseDto Input { get; set; }

        public IEnumerable<string> MuscleGroups { get; set; }
        public string ErrorMessage { get; set; }

        public void OnGet()
        {
            MuscleGroups = ExerciseInfoProvider.GetMuscleGroups();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            MuscleGroups = ExerciseInfoProvider.GetMuscleGroups();

            if (!ModelState.IsValid)
                return Page();

            try
            {
                await _service.CreateAsync(Input);
                return RedirectToPage("/Exercises/Index");
            }
            catch (ArgumentException ex)
            {
                ErrorMessage = ex.Message;
                return Page();
            }
        }
    }
}
