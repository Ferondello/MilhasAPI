using MilhasAPI.Models;

namespace MilhasAPI.Services.Interfaces;

public interface IRewardTransactionService
{
    Task<IEnumerable<RewardTransaction>> GetAllAsync();
    Task<RewardTransaction?> GetByIdAsync(int id);
    Task<IEnumerable<RewardTransaction>> GetByUserIdAsync(int userId);
    Task<RewardTransaction> CreateAsync(RewardTransaction transaction);
    Task<bool> DeleteAsync(int id);
}
