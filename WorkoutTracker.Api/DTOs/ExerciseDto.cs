namespace WorkoutTracker.Api.DTOs
{
    // Used for creating an exercise (input)
    public class CreateExerciseDto
    {
        public string Name { get; set; }
        public string MuscleGroup { get; set; }
    }

    // Used when returning exercise data (output)
    public class ExerciseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string MuscleGroup { get; set; }
    }
}
