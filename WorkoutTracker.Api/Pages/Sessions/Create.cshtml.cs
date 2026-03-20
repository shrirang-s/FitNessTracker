using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WorkoutTracker.Api.DTOs;
using WorkoutTracker.Api.Services;

namespace WorkoutTracker.Api.Pages.Sessions
{
    public class CreateSessionModel : PageModel
    {
        private readonly IWorkoutSessionService _service;

        public CreateSessionModel(IWorkoutSessionService service)
        {
            _service = service;
        }

        [BindProperty]
        public CreateWorkoutSessionDto Input { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var created = await _service.CreateAsync(Input);
            return RedirectToPage("/Sessions/Details", new { sessionId = created.Id });
        }
    }
}
