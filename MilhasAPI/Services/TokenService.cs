using MilhasAPI.Models;
using MilhasAPI.Services.Interfaces;

namespace MilhasAPI.Services;

/// <summary>
/// Implementação atual: token opaco no formato "id_email_guid".
/// Centralizado aqui para que a estratégia possa evoluir (ex.: JWT assinado)
/// sem impacto nos controllers que apenas dependem de <see cref="ITokenService"/>.
/// </summary>
public class TokenService : ITokenService
{
    public string GenerateToken(User user)
        => $"{user.Id}_{user.Email}_{Guid.NewGuid():N}";
}
