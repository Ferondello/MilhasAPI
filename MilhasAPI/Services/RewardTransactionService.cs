using MilhasAPI.Models;
using MilhasAPI.Repositories.Interfaces;
using MilhasAPI.Services.Interfaces;

namespace MilhasAPI.Services;

public class RewardTransactionService : IRewardTransactionService
{
    private readonly IRewardTransactionRepository _transactionRepository;

    public RewardTransactionService(IRewardTransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public async Task<IEnumerable<RewardTransaction>> GetAllAsync()
        => await _transactionRepository.GetAllAsync();

    public async Task<RewardTransaction?> GetByIdAsync(int id)
        => await _transactionRepository.GetByIdAsync(id);

    public async Task<IEnumerable<RewardTransaction>> GetByUserIdAsync(int userId)
        => await _transactionRepository.GetByUserIdAsync(userId);

    public async Task<RewardTransaction> CreateAsync(RewardTransaction transaction)
        => await _transactionRepository.CreateAsync(transaction);

    public async Task<bool> DeleteAsync(int id)
    {
        var transaction = await _transactionRepository.GetByIdAsync(id);
        if (transaction == null) return false;

        await _transactionRepository.DeleteAsync(transaction);
        return true;
    }
}
