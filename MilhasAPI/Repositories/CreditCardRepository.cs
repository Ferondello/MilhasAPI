using Microsoft.EntityFrameworkCore;
using MilhasAPI.Data;
using MilhasAPI.Models;
using MilhasAPI.Repositories.Interfaces;

namespace MilhasAPI.Repositories;

public class CreditCardRepository : ICreditCardRepository
{
    private readonly ApplicationDbContext _db;

    public CreditCardRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<CreditCard>> GetAllAsync()
        => await _db.CreditCards.ToListAsync();

    public async Task<CreditCard?> GetByIdAsync(int id)
        => await _db.CreditCards.FindAsync(id);

    public async Task<CreditCard> CreateAsync(CreditCard card)
    {
        _db.CreditCards.Add(card);
        await _db.SaveChangesAsync();
        return card;
    }

    public async Task UpdateAsync(CreditCard card)
    {
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(CreditCard card)
    {
        _db.CreditCards.Remove(card);
        await _db.SaveChangesAsync();
    }
}
