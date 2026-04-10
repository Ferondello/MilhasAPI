using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MilhasAPI.Data;
using MilhasAPI.Models;

namespace MilhasAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RewardTransactionsController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public RewardTransactionsController(ApplicationDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RewardTransaction>>> Get()
    {
        return await _db.RewardTransactions
            .Include(r => r.User)
            .Include(r => r.CreditCard)
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RewardTransaction>> Get(int id)
    {
        var tx = await _db.RewardTransactions
            .Include(r => r.User)
            .Include(r => r.CreditCard)
            .FirstOrDefaultAsync(r => r.Id == id);
        if (tx == null) return NotFound();
        return tx;
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<RewardTransaction>>> GetByUser(int userId)
    {
        return await _db.RewardTransactions
            .Include(r => r.CreditCard)
            .Where(r => r.UserId == userId)
            .ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<RewardTransaction>> Post(RewardTransaction tx)
    {
        _db.RewardTransactions.Add(tx);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = tx.Id }, tx);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var tx = await _db.RewardTransactions.FindAsync(id);
        if (tx == null) return NotFound();
        _db.RewardTransactions.Remove(tx);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
