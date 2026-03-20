using System.Collections.Generic;
using System.Threading.Tasks;
using WorkoutTracker.Api.Models;

namespace WorkoutTracker.Api.Repositories
{
    public interface IExerciseRepository
    {
        Task<IEnumerable<Exercise>> GetAllAsync();
        Task<Exercise> GetByIdAsync(int id);
        Task<Exercise> AddAsync(Exercise exercise);
        Task<Exercise> UpdateAsync(Exercise exercise);
        Task DeleteAsync(Exercise exercise);
    }
}
