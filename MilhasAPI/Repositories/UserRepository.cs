using Microsoft.EntityFrameworkCore;
using MilhasAPI.Data;
using MilhasAPI.Models;
using MilhasAPI.Repositories.Interfaces;

namespace MilhasAPI.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _db;

    public UserRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
        => 
        
        await _db.Users
            .Include(u => u.CreditCards)
            .Include(u => u.Profile)
            .ToListAsync();

    public async Task<User?> GetByIdAsync(int id)
        => await _db.Users
            .Include(u => u.CreditCards)
            .Include(u => u.Profile)
            .FirstOrDefaultAsync(u => u.Id == id);

    public async Task<User?> GetByEmailAsync(string email)
        => await _db.Users
            .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());

    public async Task<bool> EmailExistsAsync(string email)
        => await _db.Users.AnyAsync(u => u.Email.ToLower() == email.ToLower());

    public async Task<User> CreateAsync(User user)
    {
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return user;
    }

    public async Task UpdateAsync(User user)
    {
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(User user)
    {
        _db.Users.Remove(user);
        await _db.SaveChangesAsync();
    }
}
