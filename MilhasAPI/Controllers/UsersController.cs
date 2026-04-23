using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MilhasAPI.Data;
using MilhasAPI.Models;

namespace MilhasAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public UsersController(ApplicationDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> Get()
    {
        return await _db.Users.Include(u => u.CreditCards).Include(u => u.Profile).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> Get(int id)
    {
        var user = await _db.Users.Include(u => u.CreditCards).Include(u => u.Profile).FirstOrDefaultAsync(u => u.Id == id);
        if (user == null) return NotFound();
        return user;
    }

    [HttpPost]
    public async Task<ActionResult<User>> Post(CreateUserDto dto)
    {
        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email
        };

        if (dto.CreditCards != null && dto.CreditCards.Count > 0)
        {
            user.CreditCards = dto.CreditCards.Select(c => new CreditCard
            {
                CardNumber = c.CardNumber,
                Brand = c.Brand
            }).ToList();
        }

        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, UpdateUserDto dto)
    {
        var user = await _db.Users.FindAsync(id);
        if (user == null) return NotFound();

        if (dto.Name != null) user.Name = dto.Name;
        if (dto.Email != null) user.Email = dto.Email;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var user = await _db.Users.FindAsync(id);
        if (user == null) return NotFound();
        _db.Users.Remove(user);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
