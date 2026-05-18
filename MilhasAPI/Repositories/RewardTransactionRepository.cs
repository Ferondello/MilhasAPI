using Microsoft.EntityFrameworkCore;
using MilhasAPI.Data;
using MilhasAPI.Models;
using MilhasAPI.Repositories.Interfaces;

namespace MilhasAPI.Repositories;

public class RewardTransactionRepository : IRewardTransactionRepository
{
    private readonly ApplicationDbContext _db;

    public RewardTransactionRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<RewardTransaction>> GetAllAsync()
        => await _db.RewardTransactions
            .Include(r => r.User)
            .Include(r => r.CreditCard)
            .ToListAsync();

    public async Task<RewardTransaction?> GetByIdAsync(int id)
        => await _db.RewardTransactions
            .Include(r => r.User)
            .Include(r => r.CreditCard)
            .FirstOrDefaultAsync(r => r.Id == id);

    public async Task<IEnumerable<RewardTransaction>> GetByUserIdAsync(int userId)
        => await _db.RewardTransactions
            .Include(r => r.CreditCard)
            .Where(r => r.UserId == userId)
            .ToListAsync();

    public async Task<RewardTransaction> CreateAsync(RewardTransaction transaction)
    {
        _db.RewardTransactions.Add(transaction);
        await _db.SaveChangesAsync();
        return transaction;
    }

    public async Task DeleteAsync(RewardTransaction transaction)
    {
        _db.RewardTransactions.Remove(transaction);
        await _db.SaveChangesAsync();
    }
}
