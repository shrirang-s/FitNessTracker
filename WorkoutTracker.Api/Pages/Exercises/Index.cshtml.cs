using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WorkoutTracker.Api.DTOs;
using WorkoutTracker.Api.Services;

namespace WorkoutTracker.Api.Pages.Exercises
{
    public class ExercisesIndexModel : PageModel
    {
        private readonly IExerciseService _service;

        public ExercisesIndexModel(IExerciseService service)
        {
            _service = service;
        }

        public IEnumerable<ExerciseDto> Exercises { get; set; }
        public string ErrorMessage { get; set; }

        public async Task OnGetAsync()
        {
            Exercises = await _service.GetAllAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int exerciseId)
        {
            try
            {
                await _service.DeleteAsync(exerciseId);
            }
            catch (KeyNotFoundException)
            {
                // Already gone — fine to ignore
            }
            catch (DbUpdateException)
            {
                // FK constraint hit — exercise has logged sets
                Exercises = await _service.GetAllAsync();
                ErrorMessage = "Cannot delete this exercise — it has workout sets logged. Remove the sets first.";
                return Page();
            }

            return RedirectToPage();
        }
    }
}
