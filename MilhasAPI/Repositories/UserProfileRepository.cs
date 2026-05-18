using Microsoft.EntityFrameworkCore;
using MilhasAPI.Data;
using MilhasAPI.Models;
using MilhasAPI.Repositories.Interfaces;

namespace MilhasAPI.Repositories;

public class UserProfileRepository : IUserProfileRepository
{
    private readonly ApplicationDbContext _db;

    public UserProfileRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<UserProfile?> GetByUserIdAsync(int userId)
        => await _db.UserProfiles.FirstOrDefaultAsync(p => p.UserId == userId);

    public async Task<bool> ExistsForUserAsync(int userId)
        => await _db.UserProfiles.AnyAsync(p => p.UserId == userId);

    public async Task<UserProfile> CreateAsync(UserProfile profile)
    {
        _db.UserProfiles.Add(profile);
        await _db.SaveChangesAsync();
        return profile;
    }

    public async Task UpdateAsync(UserProfile profile)
    {
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(UserProfile profile)
    {
        _db.UserProfiles.Remove(profile);
        await _db.SaveChangesAsync();
    }
}
