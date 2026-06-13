using MilhasAPI.Models;

namespace MilhasAPI.Services.Interfaces;

/// <summary>
/// Responsável apenas por autenticação e gestão de credenciais.
/// Separado de <see cref="IUserService"/> (CRUD) para que cada consumidor
/// dependa só do que usa (ISP).
/// </summary>
public interface IAuthService
{
    Task<(User? User, string? Error)> RegisterAsync(RegisterDto dto);
    Task<User?> ValidateCredentialsAsync(string email, string password);
    Task<User?> GetByEmailAsync(string email);
    Task<bool> ResetPasswordAsync(string email, string newPassword);
}
