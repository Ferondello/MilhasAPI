using MilhasAPI.Models;

namespace MilhasAPI.Repositories.Interfaces;

public interface IUserProfileRepository
{
    Task<UserProfile?> GetByUserIdAsync(int userId);
    Task<bool> ExistsForUserAsync(int userId);
    Task<UserProfile> CreateAsync(UserProfile profile);
    Task UpdateAsync(UserProfile profile);
    Task DeleteAsync(UserProfile profile);
}
