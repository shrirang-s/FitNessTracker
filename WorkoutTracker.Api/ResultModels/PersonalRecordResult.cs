using System;

namespace WorkoutTracker.Api.ResultModels
{
    public class PersonalRecordResult
    {
        public int ExerciseId { get; set; }
        public string ExerciseName { get; set; }
        public double Weight { get; set; }
        public int Reps { get; set; }
        public DateTime Date { get; set; }
    }
}
