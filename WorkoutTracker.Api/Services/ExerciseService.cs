using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkoutTracker.Api.DTOs;
using WorkoutTracker.Api.Models;
using WorkoutTracker.Api.Repositories;

namespace WorkoutTracker.Api.Services
{
    public class ExerciseService : IExerciseService
    {
        private readonly IExerciseRepository _repo;

        public ExerciseService(IExerciseRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<ExerciseDto>> GetAllAsync()
        {
            var exercises = await _repo.GetAllAsync();
            return exercises.Select(e => new ExerciseDto
            {
                Id = e.Id,
                Name = e.Name,
                MuscleGroup = e.MuscleGroup
            });
        }

        public async Task<ExerciseDto> GetByIdAsync(int id)
        {
            var exercise = await _repo.GetByIdAsync(id);
            if (exercise == null) return null;

            return new ExerciseDto
            {
                Id = exercise.Id,
                Name = exercise.Name,
                MuscleGroup = exercise.MuscleGroup
            };
        }

        public async Task<ExerciseDto> CreateAsync(CreateExerciseDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Exercise name is required.");

            if (string.IsNullOrWhiteSpace(dto.MuscleGroup))
                throw new ArgumentException("Muscle group is required.");

            var exercise = new Exercise
            {
                Name = dto.Name.Trim(),
                MuscleGroup = dto.MuscleGroup.Trim()
            };

            var created = await _repo.AddAsync(exercise);

            return new ExerciseDto
            {
                Id = created.Id,
                Name = created.Name,
                MuscleGroup = created.MuscleGroup
            };
        }

        public async Task<ExerciseDto> UpdateAsync(int id, CreateExerciseDto dto)
        {
            var exercise = await _repo.GetByIdAsync(id);
            if (exercise == null)
                throw new KeyNotFoundException($"Exercise {id} not found.");

            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Exercise name is required.");

            if (string.IsNullOrWhiteSpace(dto.MuscleGroup))
                throw new ArgumentException("Muscle group is required.");

            exercise.Name = dto.Name.Trim();
            exercise.MuscleGroup = dto.MuscleGroup.Trim();

            var updated = await _repo.UpdateAsync(exercise);
            return new ExerciseDto { Id = updated.Id, Name = updated.Name, MuscleGroup = updated.MuscleGroup };
        }

        public async Task DeleteAsync(int id)
        {
            var exercise = await _repo.GetByIdAsync(id);
            if (exercise == null)
                throw new KeyNotFoundException($"Exercise {id} not found.");

            // EF will throw a DbUpdateException if this exercise has sets logged
            // (due to OnDelete(DeleteBehavior.Restrict) in AppDbContext).
            // We catch that in the controller/page and return a friendly message.
            await _repo.DeleteAsync(exercise);
        }
    }
}
