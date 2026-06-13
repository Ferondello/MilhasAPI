using MilhasAPI.Models;

namespace MilhasAPI.Services.Interfaces;

/// <summary>
/// Abstrai a geração do token de autenticação. A implementação atual produz
/// um token opaco simples; trocar por JWT é uma mudança isolada nesta abstração,
/// sem tocar nos controllers.
/// </summary>
public interface ITokenService
{
    string GenerateToken(User user);
}
