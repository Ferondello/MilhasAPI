using Microsoft.AspNetCore.Mvc;
using MilhasAPI.Models;
using MilhasAPI.Services.Interfaces;

namespace MilhasAPI.Controllers;

[ApiController]
[Route("api/users/{userId}/profile")]
public class UserProfilesController : ControllerBase
{
    private readonly IUserProfileService _profileService;

    public UserProfilesController(IUserProfileService profileService)
    {
        _profileService = profileService;
    }

    [HttpGet]
    public async Task<ActionResult<UserProfile>> Get(int userId)
    {
        var profile = await _profileService.GetByUserIdAsync(userId);
        if (profile == null) return NotFound();
        return Ok(profile);
    }

    [HttpPost]
    public async Task<ActionResult<UserProfile>> Create(int userId, CreateUserProfileDto dto)
    {
        var (profile, error, isConflict) = await _profileService.CreateAsync(userId, dto);

        if (error != null)
            return isConflict ? Conflict(error) : NotFound(error);

        return CreatedAtAction(nameof(Get), new { userId }, profile);
    }

    [HttpPut]
    public async Task<IActionResult> Update(int userId, UpdateUserProfileDto dto)
    {
        var updated = await _profileService.UpdateAsync(userId, dto);
        if (!updated) return NotFound("Perfil não encontrado para este usuário.");
        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int userId)
    {
        var deleted = await _profileService.DeleteAsync(userId);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
