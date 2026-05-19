using MilhasAPI.Models;

namespace MilhasAPI.Services.Interfaces;

public interface IMilesGoalService
{
    Task<IEnumerable<MilesGoal>> GetByUserIdAsync(int userId);
    Task<(MilesGoal? Goal, string? Error)> CreateAsync(CreateMilesGoalDto dto);
    Task<bool> DeleteAsync(int id);
}
