using System.Collections.Generic;
using System.Threading.Tasks;
using WorkoutTracker.Api.DTOs;

namespace WorkoutTracker.Api.Services
{
    public interface IExerciseService
    {
        Task<IEnumerable<ExerciseDto>> GetAllAsync();
        Task<ExerciseDto> GetByIdAsync(int id);
        Task<ExerciseDto> CreateAsync(CreateExerciseDto dto);
        Task<ExerciseDto> UpdateAsync(int id, CreateExerciseDto dto);
        Task DeleteAsync(int id);
    }
}
