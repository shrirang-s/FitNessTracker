using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WorkoutTracker.Api.DTOs;
using WorkoutTracker.Api.Services;

namespace WorkoutTracker.Api.Pages.Sessions
{
    public class SessionDetailsModel : PageModel
    {
        private readonly IWorkoutSessionService _sessionService;
        private readonly IWorkoutSetService _setService;

        public SessionDetailsModel(IWorkoutSessionService sessionService, IWorkoutSetService setService)
        {
            _sessionService = sessionService;
            _setService = setService;
        }

        // Bound from query string on both GET and POST
        [BindProperty(SupportsGet = true)]
        public int SessionId { get; set; }

        public WorkoutSessionDetailDto Session { get; set; }
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Session = await _sessionService.GetByIdAsync(SessionId);
            if (Session == null) return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteSetAsync(int setId)
        {
            try
            {
                await _setService.DeleteSetAsync(setId);
            }
            catch (KeyNotFoundException) { }

            return RedirectToPage(new { sessionId = SessionId });
        }
    }
}
