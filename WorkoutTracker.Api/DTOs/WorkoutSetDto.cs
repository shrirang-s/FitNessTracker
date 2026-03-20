namespace WorkoutTracker.Api.DTOs
{
    public class CreateWorkoutSetDto
    {
        public int ExerciseId { get; set; }
        public double Weight { get; set; }
        public int Reps { get; set; }
        public int SetNumber { get; set; }
    }

    public class WorkoutSetDto
    {
        public int Id { get; set; }
        public int ExerciseId { get; set; }
        public string ExerciseName { get; set; }   // denormalized for convenience
        public double Weight { get; set; }
        public int Reps { get; set; }
        public int SetNumber { get; set; }
    }
}
