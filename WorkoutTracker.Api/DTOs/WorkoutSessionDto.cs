using System;
using System.Collections.Generic;

namespace WorkoutTracker.Api.DTOs
{
    public class CreateWorkoutSessionDto
    {
        public DateTime Date { get; set; }
        public string Notes { get; set; }
    }

    public class WorkoutSessionDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Notes { get; set; }
    }

    // Used when viewing a session with all its sets included
    public class WorkoutSessionDetailDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Notes { get; set; }
        public List<WorkoutSetDto> Sets { get; set; }
    }
}
