using System.Collections.Generic;
using System.Threading.Tasks;
using WorkoutTracker.Api.DTOs;

namespace WorkoutTracker.Api.Services
{
    public interface IWorkoutSessionService
    {
        Task<IEnumerable<WorkoutSessionDto>> GetAllAsync();
        Task<WorkoutSessionDetailDto> GetByIdAsync(int id);
        Task<WorkoutSessionDto> CreateAsync(CreateWorkoutSessionDto dto);
        Task<WorkoutSessionDto> UpdateAsync(int id, CreateWorkoutSessionDto dto);
    }
}
