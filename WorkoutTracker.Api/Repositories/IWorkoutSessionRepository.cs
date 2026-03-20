using System.Collections.Generic;
using System.Threading.Tasks;
using WorkoutTracker.Api.Models;

namespace WorkoutTracker.Api.Repositories
{
    public interface IWorkoutSessionRepository
    {
        Task<IEnumerable<WorkoutSession>> GetAllAsync();
        Task<WorkoutSession> GetByIdWithSetsAsync(int id);
        Task<WorkoutSession> AddAsync(WorkoutSession session);
        Task<WorkoutSession> UpdateAsync(WorkoutSession session);
    }
}
