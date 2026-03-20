using System.Collections.Generic;
using System.Threading.Tasks;
using WorkoutTracker.Api.DTOs;

namespace WorkoutTracker.Api.Services
{
    public interface IWorkoutSetService
    {
        Task<WorkoutSetDto> GetByIdAsync(int id);
        Task<WorkoutSetDto> AddSetToSessionAsync(int sessionId, CreateWorkoutSetDto dto);
        Task<WorkoutSetDto> UpdateSetAsync(int setId, CreateWorkoutSetDto dto);
        Task DeleteSetAsync(int setId);
        Task<IEnumerable<WorkoutSetDto>> GetExerciseHistoryAsync(int exerciseId);
    }
}
