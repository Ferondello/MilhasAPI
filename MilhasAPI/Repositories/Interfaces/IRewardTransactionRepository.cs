using MilhasAPI.Models;

namespace MilhasAPI.Repositories.Interfaces;

public interface IRewardTransactionRepository
{
    Task<IEnumerable<RewardTransaction>> GetAllAsync();
    Task<RewardTransaction?> GetByIdAsync(int id);
    Task<IEnumerable<RewardTransaction>> GetByUserIdAsync(int userId);
    Task<IEnumerable<RewardTransaction>> GetByCardIdAsync(int cardId);
    Task<RewardTransaction> CreateAsync(RewardTransaction transaction);
    Task DeleteAsync(RewardTransaction transaction);
}
