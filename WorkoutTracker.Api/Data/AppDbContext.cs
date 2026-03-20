using Microsoft.EntityFrameworkCore;
using WorkoutTracker.Api.Models;

namespace WorkoutTracker.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<WorkoutSession> WorkoutSessions { get; set; }
        public DbSet<WorkoutSet> WorkoutSets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Exercise constraints
            modelBuilder.Entity<Exercise>()
                .Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Exercise>()
                .Property(e => e.MuscleGroup)
                .IsRequired()
                .HasMaxLength(50);

            // WorkoutSet → WorkoutSession FK (restrict delete so sessions
            // can't be deleted while sets exist — prevents orphaned data)
            modelBuilder.Entity<WorkoutSet>()
                .HasOne(ws => ws.WorkoutSession)
                .WithMany(s => s.WorkoutSets)
                .HasForeignKey(ws => ws.WorkoutSessionId)
                .OnDelete(DeleteBehavior.Cascade);

            // WorkoutSet → Exercise FK (restrict so exercises with history
            // can't be accidentally deleted)
            modelBuilder.Entity<WorkoutSet>()
                .HasOne(ws => ws.Exercise)
                .WithMany(e => e.WorkoutSets)
                .HasForeignKey(ws => ws.ExerciseId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
