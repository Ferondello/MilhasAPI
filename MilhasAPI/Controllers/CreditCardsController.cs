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
        var card = await _db.CreditCards.FindAsync(id);
        if (card == null) return NotFound();
        return card;
    }

    [HttpPost]
    public async Task<ActionResult<CreditCard>> Post(CreateCreditCardDto dto)
    {
        if (!await _db.Users.AnyAsync(u => u.Id == dto.UserId))
            return NotFound("Usuário não encontrado.");

        var card = new CreditCard
        {
            CardNumber = dto.CardNumber,
            Brand = dto.Brand,
            UserId = dto.UserId
        };

        _db.CreditCards.Add(card);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = card.Id }, card);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, UpdateCreditCardDto dto)
    {
        var card = await _db.CreditCards.FindAsync(id);
        if (card == null) return NotFound();

        if (dto.CardNumber != null) card.CardNumber = dto.CardNumber;
        if (dto.Brand != null) card.Brand = dto.Brand;

        await _db.SaveChangesAsync();
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
