using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkoutTracker.Api.DTOs;
using WorkoutTracker.Api.Models;
using WorkoutTracker.Api.Repositories;

namespace WorkoutTracker.Api.Services
{
    public class WorkoutSetService : IWorkoutSetService
    {
        private readonly IWorkoutSetRepository _setRepo;
        private readonly IWorkoutSessionRepository _sessionRepo;
        private readonly IExerciseRepository _exerciseRepo;

        public WorkoutSetService(
            IWorkoutSetRepository setRepo,
            IWorkoutSessionRepository sessionRepo,
            IExerciseRepository exerciseRepo)
        {
            _setRepo = setRepo;
            _sessionRepo = sessionRepo;
            _exerciseRepo = exerciseRepo;
        }

        public async Task<WorkoutSetDto> GetByIdAsync(int id)
        {
            var set = await _setRepo.GetByIdAsync(id);
            if (set == null) return null;

            return new WorkoutSetDto
            {
                Id = set.Id,
                ExerciseId = set.ExerciseId,
                ExerciseName = set.Exercise.Name,
                Weight = set.Weight,
                Reps = set.Reps,
                SetNumber = set.SetNumber
            };
        }

        public async Task<WorkoutSetDto> AddSetToSessionAsync(int sessionId, CreateWorkoutSetDto dto)
        {
            // Validate session exists
            var session = await _sessionRepo.GetByIdWithSetsAsync(sessionId);
            if (session == null)
                throw new KeyNotFoundException($"Session {sessionId} not found.");

            // Validate exercise exists
            var exercise = await _exerciseRepo.GetByIdAsync(dto.ExerciseId);
            if (exercise == null)
                throw new KeyNotFoundException($"Exercise {dto.ExerciseId} not found.");

            if (dto.Reps <= 0)
                throw new ArgumentException("Reps must be greater than 0.");

            if (dto.Weight < 0)
                throw new ArgumentException("Weight cannot be negative.");

            var set = new WorkoutSet
            {
                WorkoutSessionId = sessionId,
                ExerciseId = dto.ExerciseId,
                Weight = dto.Weight,
                Reps = dto.Reps,
                SetNumber = dto.SetNumber
            };

            var created = await _setRepo.AddAsync(set);

            return new WorkoutSetDto
            {
                Id = created.Id,
                ExerciseId = created.ExerciseId,
                ExerciseName = exercise.Name,
                Weight = created.Weight,
                Reps = created.Reps,
                SetNumber = created.SetNumber
            };
        }

        public async Task<WorkoutSetDto> UpdateSetAsync(int setId, CreateWorkoutSetDto dto)
        {
            var set = await _setRepo.GetByIdAsync(setId);
            if (set == null)
                throw new KeyNotFoundException($"Set {setId} not found.");

            var exercise = await _exerciseRepo.GetByIdAsync(dto.ExerciseId);
            if (exercise == null)
                throw new KeyNotFoundException($"Exercise {dto.ExerciseId} not found.");

            if (dto.Reps <= 0)
                throw new ArgumentException("Reps must be greater than 0.");

            if (dto.Weight < 0)
                throw new ArgumentException("Weight cannot be negative.");

            set.ExerciseId = dto.ExerciseId;
            set.Weight = dto.Weight;
            set.Reps = dto.Reps;
            set.SetNumber = dto.SetNumber;

            var updated = await _setRepo.UpdateAsync(set);
            return new WorkoutSetDto
            {
                Id = updated.Id,
                ExerciseId = updated.ExerciseId,
                ExerciseName = exercise.Name,
                Weight = updated.Weight,
                Reps = updated.Reps,
                SetNumber = updated.SetNumber
            };
        }

        public async Task DeleteSetAsync(int setId)
        {
            var set = await _setRepo.GetByIdAsync(setId);
            if (set == null)
                throw new KeyNotFoundException($"Set {setId} not found.");

            await _setRepo.DeleteAsync(set);
        }

        public async Task<IEnumerable<WorkoutSetDto>> GetExerciseHistoryAsync(int exerciseId)
        {
            var sets = await _setRepo.GetByExerciseIdAsync(exerciseId);
            return sets.Select(ws => new WorkoutSetDto
            {
                Id = ws.Id,
                ExerciseId = ws.ExerciseId,
                ExerciseName = ws.Exercise.Name,
                Weight = ws.Weight,
                Reps = ws.Reps,
                SetNumber = ws.SetNumber
            });
        }
    }
}
