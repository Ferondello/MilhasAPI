using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MilhasAPI.Data;
using MilhasAPI.Models;

namespace MilhasAPI.Controllers;

[ApiController]
[Route("api/users/{userId}/profile")]
public class UserProfilesController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public UserProfilesController(ApplicationDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<UserProfile>> Get(int userId)
    {
        var profile = await _db.UserProfiles.FirstOrDefaultAsync(p => p.UserId == userId);
        if (profile == null) return NotFound();
        return profile;
    }

    [HttpPost]
    public async Task<ActionResult<UserProfile>> Create(int userId, CreateUserProfileDto dto)
    {
        if (!await _db.Users.AnyAsync(u => u.Id == userId))
            return NotFound("Usuário não encontrado.");

        if (await _db.UserProfiles.AnyAsync(p => p.UserId == userId))
            return Conflict("Este usuário já possui um perfil cadastrado.");

        var profile = new UserProfile
        {
            UserId = userId,
            MonthlyIncome = dto.MonthlyIncome,
            InvestmentProfile = dto.InvestmentProfile,
            MonthlyCardSpending = dto.MonthlyCardSpending,
            AnnualCardFeeBudget = dto.AnnualCardFeeBudget,
            NumberOfCreditCards = dto.NumberOfCreditCards,
            TravelFrequency = dto.TravelFrequency,
            PreferredCabinClass = dto.PreferredCabinClass,
            PreferredLoyaltyProgram = dto.PreferredLoyaltyProgram,
            CurrentMilesBalance = dto.CurrentMilesBalance,
            MonthlyMilesGoal = dto.MonthlyMilesGoal,
            PrefersDomesticTravel = dto.PrefersDomesticTravel,
            PrefersInternationalTravel = dto.PrefersInternationalTravel,
            MaxMilePurchasePrice = dto.MaxMilePurchasePrice,
            InterestedInCardUpgrades = dto.InterestedInCardUpgrades,
            InterestedInMilesTransferPromos = dto.InterestedInMilesTransferPromos
        };

        _db.UserProfiles.Add(profile);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(Get), new { userId }, profile);
    }

    [HttpPut]
    public async Task<IActionResult> Update(int userId, UpdateUserProfileDto dto)
    {
        var profile = await _db.UserProfiles.FirstOrDefaultAsync(p => p.UserId == userId);
        if (profile == null) return NotFound("Perfil não encontrado para este usuário.");

        if (dto.MonthlyIncome.HasValue) profile.MonthlyIncome = dto.MonthlyIncome.Value;
        if (dto.InvestmentProfile.HasValue) profile.InvestmentProfile = dto.InvestmentProfile.Value;
        if (dto.MonthlyCardSpending.HasValue) profile.MonthlyCardSpending = dto.MonthlyCardSpending.Value;
        if (dto.AnnualCardFeeBudget.HasValue) profile.AnnualCardFeeBudget = dto.AnnualCardFeeBudget.Value;
        if (dto.NumberOfCreditCards.HasValue) profile.NumberOfCreditCards = dto.NumberOfCreditCards.Value;
        if (dto.TravelFrequency.HasValue) profile.TravelFrequency = dto.TravelFrequency.Value;
        if (dto.PreferredCabinClass.HasValue) profile.PreferredCabinClass = dto.PreferredCabinClass.Value;
        if (dto.PreferredLoyaltyProgram.HasValue) profile.PreferredLoyaltyProgram = dto.PreferredLoyaltyProgram.Value;
        if (dto.CurrentMilesBalance.HasValue) profile.CurrentMilesBalance = dto.CurrentMilesBalance.Value;
        if (dto.MonthlyMilesGoal.HasValue) profile.MonthlyMilesGoal = dto.MonthlyMilesGoal.Value;
        if (dto.PrefersDomesticTravel.HasValue) profile.PrefersDomesticTravel = dto.PrefersDomesticTravel.Value;
        if (dto.PrefersInternationalTravel.HasValue) profile.PrefersInternationalTravel = dto.PrefersInternationalTravel.Value;
        if (dto.MaxMilePurchasePrice.HasValue) profile.MaxMilePurchasePrice = dto.MaxMilePurchasePrice.Value;
        if (dto.InterestedInCardUpgrades.HasValue) profile.InterestedInCardUpgrades = dto.InterestedInCardUpgrades.Value;
        if (dto.InterestedInMilesTransferPromos.HasValue) profile.InterestedInMilesTransferPromos = dto.InterestedInMilesTransferPromos.Value;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int userId)
    {
        var profile = await _db.UserProfiles.FirstOrDefaultAsync(p => p.UserId == userId);
        if (profile == null) return NotFound();
        _db.UserProfiles.Remove(profile);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
