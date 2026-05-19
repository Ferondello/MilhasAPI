using MilhasAPI.Models;
using MilhasAPI.Repositories.Interfaces;
using MilhasAPI.Services.Interfaces;

namespace MilhasAPI.Services;

public class MilesGoalService : IMilesGoalService
{
    private readonly IMilesGoalRepository _goalRepository;
    private readonly IUserRepository _userRepository;

    public MilesGoalService(IMilesGoalRepository goalRepository, IUserRepository userRepository)
    {
        _goalRepository = goalRepository;
        _userRepository = userRepository;
    }

    public Task<IEnumerable<MilesGoal>> GetByUserIdAsync(int userId)
        => _goalRepository.GetByUserIdAsync(userId);

    public async Task<(MilesGoal? Goal, string? Error)> CreateAsync(CreateMilesGoalDto dto)
    {
        var user = await _userRepository.GetByIdAsync(dto.UserId);
        if (user == null) return (null, "Usuário não encontrado.");

        var goal = new MilesGoal
        {
            UserId = dto.UserId,
            Name = dto.Name.Trim(),
            TargetMiles = dto.TargetMiles,
            CreatedAt = DateTime.UtcNow,
        };

        var created = await _goalRepository.CreateAsync(goal);
        return (created, null);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var goal = await _goalRepository.GetByIdAsync(id);
        if (goal == null) return false;

        await _goalRepository.DeleteAsync(goal);
        return true;
    }
}
