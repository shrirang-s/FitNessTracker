using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WorkoutTracker.Api.Data;
using WorkoutTracker.Api.Models;
using WorkoutTracker.Api.ResultModels;
using System.Linq;

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
            return await _context.WorkoutSets.FindAsync(id);
        }

        public async Task<IEnumerable<WorkoutSet>> GetByExerciseIdAsync(int exerciseId)
        {
            return await _context.WorkoutSets
                .Include(ws => ws.Exercise)
                .Where(ws => ws.ExerciseId == exerciseId)
                .ToListAsync();
        }

        public async Task<WorkoutSet> AddAsync(WorkoutSet workOutSet)
        {
            _context.WorkoutSets.Add(workOutSet);
            await _context.SaveChangesAsync();
            return workOutSet;
        }

        public async Task<WorkoutSet> UpdateAsync(WorkoutSet workOutSet)
        {
            _context.WorkoutSets.Update(workOutSet);
            await _context.SaveChangesAsync();
            return workOutSet;
        }

        public async Task DeleteAsync(WorkoutSet workOutSet)
        {
            _context.WorkoutSets.Remove(workOutSet);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<PersonalRecordResult>> GetAllPersonalRecordsAsync()
        {
        var sets = await _context.WorkoutSets                                                                                               
            .Include(ws => ws.Exercise)                                                                                                     
            .Include(ws => ws.WorkoutSession)
            .ToListAsync();                                                                                                                 
                  
        return sets                                                                                                                         
            .GroupBy(g => g.ExerciseId)
            .Select(group => group                                                                                                          
                .OrderByDescending(s => s.Weight)
                .ThenByDescending(s => s.Reps)
                .First())                                                                                                                   
            .Select(s => new PersonalRecordResult { ExerciseId = s.ExerciseId, Weight = s.Weight, Reps = s.Reps, ExerciseName =       
                s.Exercise.Name, Date=s.WorkoutSession.Date })
            .ToList();   
        }
    }
}
