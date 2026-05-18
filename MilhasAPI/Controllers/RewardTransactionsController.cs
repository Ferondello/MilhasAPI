using Microsoft.AspNetCore.Mvc;
using MilhasAPI.Models;
using MilhasAPI.Services.Interfaces;

namespace MilhasAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RewardTransactionsController : ControllerBase
{
    private readonly IRewardTransactionService _transactionService;

    public RewardTransactionsController(IRewardTransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RewardTransaction>>> Get()
        => Ok(await _transactionService.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<ActionResult<RewardTransaction>> Get(int id)
    {
        var tx = await _transactionService.GetByIdAsync(id);
        if (tx == null) return NotFound();
        return Ok(tx);
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<RewardTransaction>>> GetByUser(int userId)
        => Ok(await _transactionService.GetByUserIdAsync(userId));

    [HttpPost]
    public async Task<ActionResult<RewardTransaction>> Post(RewardTransaction tx)
    {
        var created = await _transactionService.CreateAsync(tx);
        return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _transactionService.DeleteAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
