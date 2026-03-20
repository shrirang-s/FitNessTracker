using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WorkoutTracker.Api.DTOs;
using WorkoutTracker.Api.Services;

namespace WorkoutTracker.Api.Pages.Sessions
{
    public class EditSessionModel : PageModel
    {
        private readonly IWorkoutSessionService _service;

        public EditSessionModel(IWorkoutSessionService service)
        {
            _service = service;
        }

        [BindProperty(SupportsGet = true)]
        public int SessionId { get; set; }

        [BindProperty]
        public CreateWorkoutSessionDto Input { get; set; }

        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var session = await _service.GetByIdAsync(SessionId);
            if (session == null) return NotFound();

            // Pre-populate form with existing values
            Input = new CreateWorkoutSessionDto
            {
                Date = session.Date,
                Notes = session.Notes
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            try
            {
                await _service.UpdateAsync(SessionId, Input);
                return RedirectToPage("/Sessions/Details", new { sessionId = SessionId });
            }
            catch (KeyNotFoundException ex)
            {
                ErrorMessage = ex.Message;
                return Page();
            }
        }
    }
}
