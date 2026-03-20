using System.Collections.Generic;

namespace WorkoutTracker.Api.Models
{
    public class Exercise
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string MuscleGroup { get; set; }

        // Navigation property — EF uses this to load related sets
        public ICollection<WorkoutSet> WorkoutSets { get; set; }
    }
}
