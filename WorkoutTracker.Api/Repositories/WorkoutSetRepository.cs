using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WorkoutTracker.Api.Data;
using WorkoutTracker.Api.Models;

namespace WorkoutTracker.Api.Repositories
{
    public class WorkoutSetRepository : IWorkoutSetRepository
    {
        private readonly AppDbContext _context;

        public WorkoutSetRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<WorkoutSet> GetByIdAsync(int id)
        {
            return await _context.WorkoutSets
                .Include(ws => ws.Exercise)
                .FirstOrDefaultAsync(ws => ws.Id == id);
        }

        public async Task<WorkoutSet> AddAsync(WorkoutSet set)
        {
            _context.WorkoutSets.Add(set);
            await _context.SaveChangesAsync();
            return set;
        }

        public async Task<WorkoutSet> UpdateAsync(WorkoutSet set)
        {
            _context.WorkoutSets.Update(set);
            await _context.SaveChangesAsync();
            return set;
        }

        public async Task DeleteAsync(WorkoutSet set)
        {
            _context.WorkoutSets.Remove(set);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<WorkoutSet>> GetByExerciseIdAsync(int exerciseId)
        {
            // Used for exercise history — loads session date for context
            return await _context.WorkoutSets
                .Include(ws => ws.WorkoutSession)
                .Include(ws => ws.Exercise)
                .Where(ws => ws.ExerciseId == exerciseId)
                .OrderByDescending(ws => ws.WorkoutSession.Date)
                .ToListAsync();
        }
    }
}
