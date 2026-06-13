using MilhasAPI.Models;
using MilhasAPI.Repositories.Interfaces;
using MilhasAPI.Services.Interfaces;

namespace MilhasAPI.Services;

/// <summary>
/// Concentra as regras de autenticação: registro, validação de credenciais e
/// redefinição de senha. O hashing de senha (BCrypt) é detalhe interno desta classe.
/// </summary>
public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;

    public AuthService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<(User? User, string? Error)> RegisterAsync(RegisterDto dto)
    {
        if (await _userRepository.EmailExistsAsync(dto.Email))
            return (null, "Este e-mail já está cadastrado.");

        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
        };

        var created = await _userRepository.CreateAsync(user);
        return (created, null);
    }

    public async Task<User?> ValidateCredentialsAsync(string email, string password)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null || user.PasswordHash == null) return null;

        return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash) ? user : null;
    }

    public Task<User?> GetByEmailAsync(string email)
        => _userRepository.GetByEmailAsync(email);

    public async Task<bool> ResetPasswordAsync(string email, string newPassword)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null) return false;

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
        await _userRepository.UpdateAsync(user);
        return true;
    }
}
