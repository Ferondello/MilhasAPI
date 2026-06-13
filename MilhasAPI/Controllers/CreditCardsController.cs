using Microsoft.AspNetCore.Mvc;
using MilhasAPI.Models;
using MilhasAPI.Services.Interfaces;

namespace MilhasAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CreditCardsController : ControllerBase
{
    private readonly ICreditCardService _cardService;
    private readonly IRewardTransactionService _transactionService;

    public CreditCardsController(ICreditCardService cardService, IRewardTransactionService transactionService)
    {
        _cardService = cardService;
        _transactionService = transactionService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CreditCardResponseDto>>> Get()
        => Ok(await _cardService.GetAllAsync());

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<CreditCardResponseDto>>> GetByUser(int userId)
        => Ok(await _cardService.GetByUserIdAsync(userId));

    [HttpGet("{id}")]
    public async Task<ActionResult<CreditCardResponseDto>> Get(int id)
    {
        var card = await _cardService.GetByIdAsync(id);
        if (card == null) return NotFound();
        return Ok(card);
    }

    [HttpGet("{id}/transactions")]
    public async Task<IActionResult> GetTransactions(int id)
    {
        var card = await _cardService.GetByIdAsync(id);
        if (card == null) return NotFound();

        var transactions = await _transactionService.GetByCardIdAsync(id);
        var result = transactions.Select(t => new
        {
            t.Id,
            t.Date,
            t.Amount,
            t.MilesEarned,
            t.CreditCardId,
        });

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<CreditCardResponseDto>> Post(CreateCreditCardDto dto)
    {
        var (card, error) = await _cardService.CreateAsync(dto);
        if (error != null) return NotFound(new { message = error });
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
