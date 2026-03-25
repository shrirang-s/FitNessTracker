using System.Collections.Generic;
using System.Threading.Tasks;
using WorkoutTracker.Api.Models;
using WorkoutTracker.Api.ResultModels;

namespace WorkoutTracker.Api.Repositories
{
    public interface IWorkoutSetRepository
    {
        Task<WorkoutSet> GetByIdAsync(int id);
        Task<IEnumerable<WorkoutSet>> GetByExerciseIdAsync(int exerciseId);
        Task<WorkoutSet> AddAsync(WorkoutSet set);
        Task<WorkoutSet> UpdateAsync(WorkoutSet set);
        Task<IEnumerable<PersonalRecordResult>> GetAllPersonalRecordsAsync();
        Task DeleteAsync(WorkoutSet set);
    }
}
