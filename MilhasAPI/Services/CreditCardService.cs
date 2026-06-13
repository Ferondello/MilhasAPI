using MilhasAPI.Models;
using MilhasAPI.Repositories.Interfaces;
using MilhasAPI.Services.Interfaces;
using MilhasAPI.Utils;

namespace MilhasAPI.Services;

public class CreditCardService : ICreditCardService
{
    private readonly ICreditCardRepository _cardRepository;
    private readonly IUserRepository _userRepository;

    public CreditCardService(ICreditCardRepository cardRepository, IUserRepository userRepository)
    {
        _cardRepository = cardRepository;
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<CreditCardResponseDto>> GetAllAsync()
    {
        var cards = await _cardRepository.GetAllAsync();
        return cards.Select(ToResponseDto).ToList();
    }

    public async Task<IEnumerable<CreditCardResponseDto>> GetByUserIdAsync(int userId)
    {
        var cards = await _cardRepository.GetByUserIdAsync(userId);
        return cards.Select(ToResponseDto).ToList();
    }

    public async Task<CreditCardResponseDto?> GetByIdAsync(int id)
    {
        var card = await _cardRepository.GetByIdAsync(id);
        return card == null ? null : ToResponseDto(card);
    }

    public async Task<(CreditCardResponseDto? Card, string? Error)> CreateAsync(CreateCreditCardDto dto)
    {
        var userExists = await _userRepository.GetByIdAsync(dto.UserId);
        if (userExists == null)
            return (null, "Usuário não encontrado.");

        var card = new CreditCard
        {
            CardNumber = dto.CardNumber,
            Brand = dto.Brand,
            Program = dto.Program,
            UserId = dto.UserId
        };

        var created = await _cardRepository.CreateAsync(card);
        return (ToResponseDto(created), null);
    }

    public async Task<bool> UpdateAsync(int id, UpdateCreditCardDto dto)
    {
        var card = await _cardRepository.GetByIdAsync(id);
        if (card == null) return false;

        if (dto.CardNumber != null) card.CardNumber = dto.CardNumber;
        if (dto.Brand != null) card.Brand = dto.Brand;
        if (dto.Program != null) card.Program = dto.Program;

        await _cardRepository.UpdateAsync(card);
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var card = await _cardRepository.GetByIdAsync(id);
        if (card == null) return false;

        await _cardRepository.DeleteAsync(card);
        return true;
    }

    // Mapeia a entidade para saída, mascarando o número — sem mutar a entidade
    // rastreada pelo EF (evita persistir o valor mascarado por engano).
    private static CreditCardResponseDto ToResponseDto(CreditCard card) => new()
    {
        Id = card.Id,
        CardNumber = CardNumberMasker.Mask(card.CardNumber),
        Brand = card.Brand,
        Program = card.Program,
        UserId = card.UserId,
    };
}
