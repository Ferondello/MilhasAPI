using Microsoft.AspNetCore.Mvc;
using MilhasAPI.Models;
using MilhasAPI.Services.Interfaces;

namespace MilhasAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MilesGoalsController : ControllerBase
{
    private readonly IMilesGoalService _goalService;

    public MilesGoalsController(IMilesGoalService goalService)
    {
        _goalService = goalService;
    }

    /// <summary>Lista metas de milhas de um usuário, ordenadas por TargetMiles ascendente.</summary>
    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<MilesGoal>>> GetByUser(int userId)
        => Ok(await _goalService.GetByUserIdAsync(userId));

    [HttpPost]
    public async Task<ActionResult<MilesGoal>> Post(CreateMilesGoalDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var (goal, error) = await _goalService.CreateAsync(dto);
        if (error != null) return NotFound(new { message = error });

        return CreatedAtAction(nameof(GetByUser), new { userId = goal!.UserId }, goal);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _goalService.DeleteAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
