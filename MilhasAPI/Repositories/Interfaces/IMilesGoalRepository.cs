using MilhasAPI.Models;

namespace MilhasAPI.Repositories.Interfaces;

public interface IMilesGoalRepository
{
    Task<IEnumerable<MilesGoal>> GetByUserIdAsync(int userId);
    Task<MilesGoal?> GetByIdAsync(int id);
    Task<MilesGoal> CreateAsync(MilesGoal goal);
    Task DeleteAsync(MilesGoal goal);
}
