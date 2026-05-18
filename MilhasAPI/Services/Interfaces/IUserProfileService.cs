using MilhasAPI.Models;

namespace MilhasAPI.Services.Interfaces;

public interface IUserProfileService
{
    Task<UserProfile?> GetByUserIdAsync(int userId);
    Task<(UserProfile? Profile, string? Error, bool IsConflict)> CreateAsync(int userId, CreateUserProfileDto dto);
    Task<bool> UpdateAsync(int userId, UpdateUserProfileDto dto);
    Task<bool> DeleteAsync(int userId);
}
