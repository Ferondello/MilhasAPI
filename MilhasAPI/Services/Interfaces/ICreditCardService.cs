using MilhasAPI.Models;

namespace MilhasAPI.Services.Interfaces;

public interface ICreditCardService
{
    Task<IEnumerable<CreditCard>> GetAllAsync();
    Task<IEnumerable<CreditCard>> GetByUserIdAsync(int userId);
    Task<CreditCard?> GetByIdAsync(int id);
    Task<(CreditCard? Card, string? Error)> CreateAsync(CreateCreditCardDto dto);
    Task<bool> UpdateAsync(int id, UpdateCreditCardDto dto);
    Task<bool> DeleteAsync(int id);
}
