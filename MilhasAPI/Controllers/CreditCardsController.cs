using Microsoft.AspNetCore.Mvc;
using MilhasAPI.Models;
using MilhasAPI.Services.Interfaces;

namespace MilhasAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CreditCardsController : ControllerBase
{
    private readonly ICreditCardService _cardService;

    public CreditCardsController(ICreditCardService cardService)
    {
        _cardService = cardService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CreditCard>>> Get()
        => Ok(await _cardService.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<ActionResult<CreditCard>> Get(int id)
    {
        var card = await _cardService.GetByIdAsync(id);
        if (card == null) return NotFound();
        return Ok(card);
    }

    [HttpPost]
    public async Task<ActionResult<CreditCard>> Post(CreateCreditCardDto dto)
    {
        var (card, error) = await _cardService.CreateAsync(dto);
        if (error != null) return NotFound(error);
        return CreatedAtAction(nameof(Get), new { id = card!.Id }, card);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, UpdateCreditCardDto dto)
    {
        var updated = await _cardService.UpdateAsync(id, dto);
        if (!updated) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _cardService.DeleteAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
