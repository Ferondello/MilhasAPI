using MilhasAPI.Models;

namespace MilhasAPI.Services.Interfaces;

public interface ICreditCardService
{
    Task<IEnumerable<CreditCardResponseDto>> GetAllAsync();
    Task<CreditCardResponseDto?> GetByIdAsync(int id);
    Task<(CreditCardResponseDto? Card, string? Error)> CreateAsync(CreateCreditCardDto dto);
    Task<bool> UpdateAsync(int id, UpdateCreditCardDto dto);
    Task<bool> DeleteAsync(int id);
}
