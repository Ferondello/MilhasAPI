using MilhasAPI.Models;

namespace MilhasAPI.Services.Interfaces;

public interface IUserService
{
    Task<IEnumerable<User>> GetAllAsync();
    Task<User?> GetByIdAsync(int id);
    Task<User> CreateAsync(CreateUserDto dto);
    Task<bool> UpdateAsync(int id, UpdateUserDto dto);
    Task<bool> DeleteAsync(int id);

    // Auth
    Task<(User? User, string? Error)> RegisterAsync(RegisterDto dto);
    Task<User?> ValidateCredentialsAsync(string email, string password);
}
