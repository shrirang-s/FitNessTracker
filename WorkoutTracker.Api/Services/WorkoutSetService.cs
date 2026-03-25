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
        private readonly IWorkoutSetRepository _repo;
        private readonly IWorkoutSessionRepository _workoutSessionRepo;
        private readonly IExerciseRepository _exerciseRepo;

        public WorkoutSetService(IWorkoutSetRepository repo, IWorkoutSessionRepository workoutSessionRepo, IExerciseRepository exerciseRepo)
        {
            _repo = repo;
            _workoutSessionRepo = workoutSessionRepo;
            _exerciseRepo = exerciseRepo;
        }

        public async Task<WorkoutSetDto> GetByIdAsync(int id)
        {
            var set = await _repo.GetByIdAsync(id);
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
            var session = await _workoutSessionRepo.GetByIdWithSetsAsync(sessionId);
            if (session == null)
                throw new KeyNotFoundException($"Session {sessionId} not found.");

            var exercise = await _exerciseRepo.GetByIdAsync(dto.ExerciseId);
            if (exercise == null)
                throw new KeyNotFoundException($"Exercise {dto.ExerciseId} not found.");

            var workoutSet = new WorkoutSet
            {
                ExerciseId = dto.ExerciseId,
                Weight = dto.Weight,
                Reps = dto.Reps,
                SetNumber = dto.SetNumber,
                WorkoutSessionId = sessionId
            };

            var created = await _repo.AddAsync(workoutSet);

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
            var fetchedSet = await _repo.GetByIdAsync(setId);

            if (fetchedSet == null)
            {
                throw new KeyNotFoundException($"Set {setId} not found.");
            }
            var exercise = await _exerciseRepo.GetByIdAsync(dto.ExerciseId);
            if (exercise == null)
                throw new KeyNotFoundException($"Exercise {dto.ExerciseId} not found.");

            fetchedSet.ExerciseId = dto.ExerciseId;
            fetchedSet.Weight = dto.Weight;
            fetchedSet.Reps = dto.Reps;
            fetchedSet.SetNumber = dto.SetNumber;

            var updated = await _repo.UpdateAsync(fetchedSet);
            return new WorkoutSetDto { Id = updated.Id, ExerciseId = updated.ExerciseId, ExerciseName = exercise.Name, Weight = updated.Weight, Reps = updated.Reps, SetNumber = updated.SetNumber };
        }

        public async Task DeleteSetAsync(int setId)
        {
            var fetchedSet = await _repo.GetByIdAsync(setId);

            if (fetchedSet == null)
            {
                throw new KeyNotFoundException($"Set {setId} not found.");
            }

            await _repo.DeleteAsync(fetchedSet);
        }

        public async Task<IEnumerable<WorkoutSetDto>> GetExerciseHistoryAsync(int exerciseId)
        {
            var sets = await _repo.GetByExerciseIdAsync(exerciseId);
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

        public async Task<IEnumerable<PersonalRecordDto>> GetAllPersonalRecords()
        {
            var records = await _repo.GetAllPersonalRecordsAsync();

            return records.Select(record =>
            new PersonalRecordDto {
                ExerciseId = record.ExerciseId,
                Weight = record.Weight,
                Reps = record.Reps,
                ExerciseName = record.ExerciseName,
                Date = record.Date
            });
        }
    }
}
