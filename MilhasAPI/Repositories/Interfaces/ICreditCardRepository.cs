using MilhasAPI.Models;

namespace MilhasAPI.Repositories.Interfaces;

public interface ICreditCardRepository
{
    Task<IEnumerable<CreditCard>> GetAllAsync();
    Task<CreditCard?> GetByIdAsync(int id);
    Task<CreditCard> CreateAsync(CreditCard card);
    Task UpdateAsync(CreditCard card);
    Task DeleteAsync(CreditCard card);
}
