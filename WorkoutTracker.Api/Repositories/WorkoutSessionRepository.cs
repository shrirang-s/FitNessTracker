using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WorkoutTracker.Api.Data;
using WorkoutTracker.Api.Models;

namespace WorkoutTracker.Api.Repositories
{
    public class WorkoutSessionRepository : IWorkoutSessionRepository
    {
        private readonly AppDbContext _context;

        public WorkoutSessionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<WorkoutSession>> GetAllAsync()
        {
            return await _context.WorkoutSessions
                .OrderByDescending(s => s.Date)
                .ToListAsync();
        }

        public async Task<WorkoutSession> GetByIdWithSetsAsync(int id)
        {
            // Eager-load sets and their exercises in a single query
            return await _context.WorkoutSessions
                .Include(s => s.WorkoutSets)
                    .ThenInclude(ws => ws.Exercise)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<WorkoutSession> AddAsync(WorkoutSession session)
        {
            _context.WorkoutSessions.Add(session);
            await _context.SaveChangesAsync();
            return session;
        }

        public async Task<WorkoutSession> UpdateAsync(WorkoutSession session)
        {
            _context.WorkoutSessions.Update(session);
            await _context.SaveChangesAsync();
            return session;
        }
    }
}
