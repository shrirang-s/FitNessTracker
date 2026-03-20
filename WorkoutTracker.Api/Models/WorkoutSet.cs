namespace WorkoutTracker.Api.Models
{
    public class WorkoutSet
    {
        public int Id { get; set; }
        public int WorkoutSessionId { get; set; }
        public int ExerciseId { get; set; }
        public double Weight { get; set; }
        public int Reps { get; set; }
        public int SetNumber { get; set; }

        // Navigation properties — EF resolves FK relationships via these
        public WorkoutSession WorkoutSession { get; set; }
        public Exercise Exercise { get; set; }
    }
}
