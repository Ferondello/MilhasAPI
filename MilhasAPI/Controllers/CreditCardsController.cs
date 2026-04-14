using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MilhasAPI.Data;
using MilhasAPI.Models;

namespace MilhasAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CreditCardsController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public CreditCardsController(ApplicationDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CreditCard>>> Get()
    {
        return await _db.CreditCards.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CreditCard>> Get(int id)
    {
        var card = await _db.CreditCards.FirstOrDefaultAsync(c => c.Id == id);
        if (card == null) return NotFound();
        return card;
    }

    [HttpPost]
    public async Task<ActionResult<CreditCard>> Post(CreditCard card)
    {
        _db.CreditCards.Add(card);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = card.Id }, card);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, CreditCard updated)
    {
        if (id != updated.Id) return BadRequest();
        _db.Entry(updated).State = EntityState.Modified;
        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _db.CreditCards.AnyAsync(c => c.Id == id)) return NotFound();
            throw;
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var card = await _db.CreditCards.FindAsync(id);
        if (card == null) return NotFound();
        _db.CreditCards.Remove(card);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
