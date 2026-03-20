using System.Collections.Generic;
using System.Linq;

namespace WorkoutTracker.Api.Infrastructure
{
    public class ExerciseInfo
    {
        public string Description { get; set; }
        public string Tips { get; set; }
        public string PrimaryMuscle { get; set; }
        public string SecondaryMuscles { get; set; }
    }

    public class MuscleGroupInfo
    {
        public string Description { get; set; }
        public string ImagePath { get; set; }         // relative path under wwwroot
        public string[] MusclesInvolved { get; set; }
    }

    public static class ExerciseInfoProvider
    {
        // Known exercises → description + coaching tips
        private static readonly Dictionary<string, ExerciseInfo> _exercises =
            new Dictionary<string, ExerciseInfo>(System.StringComparer.OrdinalIgnoreCase)
        {
            ["Bench Press"] = new ExerciseInfo
            {
                Description = "A compound pushing movement performed lying on a bench. The barbell is lowered to the chest and pressed back up.",
                Tips = "Keep shoulder blades retracted and feet flat on the floor. Lower the bar under control to mid-chest.",
                PrimaryMuscle = "Pectoralis Major",
                SecondaryMuscles = "Anterior Deltoid, Triceps Brachii"
            },
            ["Squat"] = new ExerciseInfo
            {
                Description = "A fundamental lower-body compound movement. Bar rests on upper back; athlete descends until thighs are parallel to the floor.",
                Tips = "Keep knees tracking over toes. Maintain a neutral spine and brace your core throughout the movement.",
                PrimaryMuscle = "Quadriceps",
                SecondaryMuscles = "Glutes, Hamstrings, Spinal Erectors"
            },
            ["Deadlift"] = new ExerciseInfo
            {
                Description = "A hip-hinge movement lifting a barbell from the floor to a standing position. One of the best full-body strength exercises.",
                Tips = "Keep the bar close to your body. Hinge at the hips, not the waist. Drive through the floor and lock out at the top.",
                PrimaryMuscle = "Erector Spinae, Glutes",
                SecondaryMuscles = "Hamstrings, Quadriceps, Trapezius, Forearms"
            },
            ["Pull-Up"] = new ExerciseInfo
            {
                Description = "An upper-body pulling movement using bodyweight. Hang from a bar and pull until chin clears the bar.",
                Tips = "Start from a dead hang. Initiate by depressing the scapulae before bending the elbows.",
                PrimaryMuscle = "Latissimus Dorsi",
                SecondaryMuscles = "Biceps Brachii, Rear Deltoid, Rhomboids"
            },
            ["Overhead Press"] = new ExerciseInfo
            {
                Description = "A standing or seated press where the barbell is pushed from shoulder height to full lockout overhead.",
                Tips = "Keep the core tight and avoid excessive lumbar extension. Press the bar in a straight vertical path.",
                PrimaryMuscle = "Anterior Deltoid",
                SecondaryMuscles = "Triceps, Upper Pectorals, Trapezius"
            },
            ["Romanian Deadlift"] = new ExerciseInfo
            {
                Description = "A hip-hinge variation of the deadlift focusing on hamstring stretch. The bar stays close to the legs throughout.",
                Tips = "Push your hips back as you lower the bar. Feel the stretch in your hamstrings before reversing.",
                PrimaryMuscle = "Hamstrings",
                SecondaryMuscles = "Glutes, Spinal Erectors"
            },
            ["Barbell Row"] = new ExerciseInfo
            {
                Description = "A horizontal pulling movement performed bent-over. The bar is pulled into the lower chest/abdomen.",
                Tips = "Keep the torso roughly parallel to the floor. Drive the elbows back and squeeze the lats at the top.",
                PrimaryMuscle = "Latissimus Dorsi",
                SecondaryMuscles = "Rhomboids, Rear Deltoid, Biceps"
            },
            ["Tricep Dip"] = new ExerciseInfo
            {
                Description = "A bodyweight or weighted pushing movement performed on parallel bars. Targets the back of the upper arm.",
                Tips = "Keep torso upright to emphasize triceps. Lean forward to shift load toward the chest.",
                PrimaryMuscle = "Triceps Brachii",
                SecondaryMuscles = "Anterior Deltoid, Pectoralis Major"
            },
            ["Bicep Curl"] = new ExerciseInfo
            {
                Description = "An isolation movement for the biceps using a barbell or dumbbells. Elbow flexion against resistance.",
                Tips = "Keep elbows pinned at your sides. Avoid swinging the torso to generate momentum.",
                PrimaryMuscle = "Biceps Brachii",
                SecondaryMuscles = "Brachialis, Brachioradialis"
            },
            ["Plank"] = new ExerciseInfo
            {
                Description = "An isometric core exercise holding a push-up position. Builds stability and endurance in the trunk.",
                Tips = "Keep hips level — don't sag or pike. Squeeze glutes and brace the abs as if bracing for a punch.",
                PrimaryMuscle = "Rectus Abdominis, Transverse Abdominis",
                SecondaryMuscles = "Obliques, Glutes, Shoulders"
            }
        };

        // Muscle groups → diagram info
        private static readonly Dictionary<string, MuscleGroupInfo> _muscleGroups =
            new Dictionary<string, MuscleGroupInfo>(System.StringComparer.OrdinalIgnoreCase)
        {
            ["Chest"] = new MuscleGroupInfo
            {
                Description = "The pectoral muscles span the front of the upper chest. They are responsible for horizontal pushing and adduction of the arm.",
                ImagePath = "https://www.ncbi.nlm.nih.gov/books/NBK525991/bin/Gray410.jpg",
                MusclesInvolved = new[] { "Pectoralis Major", "Pectoralis Minor", "Serratus Anterior" }
            },
            ["Back"] = new MuscleGroupInfo
            {
                Description = "The back comprises the largest muscle group in the upper body. Key muscles include the lats, traps, and rhomboids, responsible for pulling and posture.",
                ImagePath = "https://upload.wikimedia.org/wikipedia/commons/c/ce/1117_Muscles_of_the_Back.png",
                MusclesInvolved = new[] { "Latissimus Dorsi", "Trapezius", "Rhomboids", "Erector Spinae", "Rear Deltoid" }
            },
            ["Legs"] = new MuscleGroupInfo
            {
                Description = "The legs include some of the largest and most powerful muscles in the body — essential for squatting, hinging, and explosive movement.",
                ImagePath = "https://www.ncbi.nlm.nih.gov/books/NBK539897/bin/Quadriceps.jpg",
                MusclesInvolved = new[] { "Quadriceps", "Hamstrings", "Glutes", "Calves", "Hip Flexors" }
            },
            ["Arms"] = new MuscleGroupInfo
            {
                Description = "The arm muscles control elbow flexion and extension. The biceps and triceps are antagonist pairs that work together for balanced arm strength.",
                ImagePath = "https://www.ncbi.nlm.nih.gov/books/NBK519538/bin/biceps__muscle.jpg",
                MusclesInvolved = new[] { "Biceps Brachii", "Triceps Brachii", "Brachialis", "Brachioradialis" }
            },
            ["Shoulders"] = new MuscleGroupInfo
            {
                Description = "The deltoid has three heads — anterior, lateral, and posterior. Shoulder training builds width and stability for overhead pressing.",
                ImagePath = "https://upload.wikimedia.org/wikipedia/commons/c/c4/Deltoid_muscle_top10.png",
                MusclesInvolved = new[] { "Anterior Deltoid", "Lateral Deltoid", "Posterior Deltoid", "Rotator Cuff" }
            },
            ["Core"] = new MuscleGroupInfo
            {
                Description = "The core stabilises the spine and transfers force between the upper and lower body. Far more than just the abs — it includes deep stabilisers.",
                ImagePath = "https://www.ncbi.nlm.nih.gov/books/NBK459392/bin/Gray397.jpg",
                MusclesInvolved = new[] { "Rectus Abdominis", "Transverse Abdominis", "Internal Oblique", "External Oblique", "Erector Spinae" }
            }
        };

        // All known muscle groups — used to populate the dropdown on the Create form
        public static IEnumerable<string> GetMuscleGroups() => _muscleGroups.Keys;

        public static ExerciseInfo GetExerciseInfo(string exerciseName)
        {
            return _exercises.TryGetValue(exerciseName, out var info) ? info : null;
        }

        public static MuscleGroupInfo GetMuscleGroupInfo(string muscleGroup)
        {
            return _muscleGroups.TryGetValue(muscleGroup, out var info) ? info : null;
        }
    }
}
