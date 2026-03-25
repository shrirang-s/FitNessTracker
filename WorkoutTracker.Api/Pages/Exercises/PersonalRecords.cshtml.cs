// Pages/Exercises/PersonalRecords.cshtml.cs
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using WorkoutTracker.Api.Services;
using WorkoutTracker.Api.DTOs;

namespace WorkoutTracker.Api.Pages.Exercises
{
    public class PersonalRecordsModel : PageModel
    {
        private readonly IWorkoutSetService _setService;

        public PersonalRecordsModel(IWorkoutSetService setService)
        {
            _setService = setService;
        }

        public IEnumerable<PersonalRecordDto> Records { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Records = await _setService.GetAllPersonalRecords();
            return Page();
        }
    }
}