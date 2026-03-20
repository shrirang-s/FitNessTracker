using System.Collections.Generic;
using System.Threading.Tasks;
using WorkoutTracker.Api.Models;

namespace WorkoutTracker.Api.Repositories
{
    public interface IWorkoutSetRepository
    {
        Task<WorkoutSet> GetByIdAsync(int id);
        Task<WorkoutSet> AddAsync(WorkoutSet set);
        Task<WorkoutSet> UpdateAsync(WorkoutSet set);
        Task DeleteAsync(WorkoutSet set);
        Task<IEnumerable<WorkoutSet>> GetByExerciseIdAsync(int exerciseId);
    }
}
