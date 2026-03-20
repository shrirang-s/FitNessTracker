using System;
using System.Collections.Generic;

namespace WorkoutTracker.Api.Models
{
    public class WorkoutSession
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Notes { get; set; }

        // Navigation property — one session has many sets
        public ICollection<WorkoutSet> WorkoutSets { get; set; }
    }
}
