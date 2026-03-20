using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkoutTracker.Api.DTOs;
using WorkoutTracker.Api.Models;
using WorkoutTracker.Api.Repositories;

namespace WorkoutTracker.Api.Services
{
    public class WorkoutSessionService : IWorkoutSessionService
    {
        private readonly IWorkoutSessionRepository _repo;

        public WorkoutSessionService(IWorkoutSessionRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<WorkoutSessionDto>> GetAllAsync()
        {
            var sessions = await _repo.GetAllAsync();
            return sessions.Select(s => new WorkoutSessionDto
            {
                Id = s.Id,
                Date = s.Date,
                Notes = s.Notes
            });
        }

        public async Task<WorkoutSessionDetailDto> GetByIdAsync(int id)
        {
            var session = await _repo.GetByIdWithSetsAsync(id);
            if (session == null) return null;

            return new WorkoutSessionDetailDto
            {
                Id = session.Id,
                Date = session.Date,
                Notes = session.Notes,
                Sets = session.WorkoutSets.Select(ws => new WorkoutSetDto
                {
                    Id = ws.Id,
                    ExerciseId = ws.ExerciseId,
                    ExerciseName = ws.Exercise.Name,
                    Weight = ws.Weight,
                    Reps = ws.Reps,
                    SetNumber = ws.SetNumber
                }).OrderBy(s => s.SetNumber).ToList()
            };
        }

        public async Task<WorkoutSessionDto> CreateAsync(CreateWorkoutSessionDto dto)
        {
            var session = new WorkoutSession
            {
                Date = dto.Date == default ? DateTime.UtcNow : dto.Date,
                Notes = dto.Notes?.Trim()
            };

            var created = await _repo.AddAsync(session);

            return new WorkoutSessionDto
            {
                Id = created.Id,
                Date = created.Date,
                Notes = created.Notes
            };
        }

        public async Task<WorkoutSessionDto> UpdateAsync(int id, CreateWorkoutSessionDto dto)
        {
            var session = await _repo.GetByIdWithSetsAsync(id);
            if (session == null)
                throw new KeyNotFoundException($"Session {id} not found.");

            session.Date = dto.Date == default ? session.Date : dto.Date;
            session.Notes = dto.Notes?.Trim();

            var updated = await _repo.UpdateAsync(session);
            return new WorkoutSessionDto { Id = updated.Id, Date = updated.Date, Notes = updated.Notes };
        }
    }
}
