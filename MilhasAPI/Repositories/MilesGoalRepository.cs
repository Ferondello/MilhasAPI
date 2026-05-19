using Microsoft.EntityFrameworkCore;
using MilhasAPI.Data;
using MilhasAPI.Models;
using MilhasAPI.Repositories.Interfaces;

namespace MilhasAPI.Repositories;

public class MilesGoalRepository : IMilesGoalRepository
{
    private readonly ApplicationDbContext _db;

    public MilesGoalRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<MilesGoal>> GetByUserIdAsync(int userId)
        => await _db.MilesGoals
            .Where(g => g.UserId == userId)
            .OrderBy(g => g.TargetMiles)
            .ToListAsync();

    public async Task<MilesGoal?> GetByIdAsync(int id)
        => await _db.MilesGoals.FindAsync(id);

    public async Task<MilesGoal> CreateAsync(MilesGoal goal)
    {
        _db.MilesGoals.Add(goal);
        await _db.SaveChangesAsync();
        return goal;
    }

    public async Task DeleteAsync(MilesGoal goal)
    {
        _db.MilesGoals.Remove(goal);
        await _db.SaveChangesAsync();
    }
}
