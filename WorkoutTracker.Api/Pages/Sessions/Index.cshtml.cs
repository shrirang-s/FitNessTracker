using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WorkoutTracker.Api.DTOs;
using WorkoutTracker.Api.Services;

namespace WorkoutTracker.Api.Pages.Sessions
{
    public class SessionsIndexModel : PageModel
    {
        private readonly IWorkoutSessionService _service;

        public SessionsIndexModel(IWorkoutSessionService service)
        {
            _service = service;
        }

        public IEnumerable<WorkoutSessionDto> Sessions { get; set; }

        public async Task OnGetAsync()
        {
            Sessions = await _service.GetAllAsync();
        }
    }
}
