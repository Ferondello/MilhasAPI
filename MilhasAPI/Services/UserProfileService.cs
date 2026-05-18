using MilhasAPI.Models;
using MilhasAPI.Repositories.Interfaces;
using MilhasAPI.Services.Interfaces;

namespace MilhasAPI.Services;

public class UserProfileService : IUserProfileService
{
    private readonly IUserProfileRepository _profileRepository;
    private readonly IUserRepository _userRepository;

    public UserProfileService(IUserProfileRepository profileRepository, IUserRepository userRepository)
    {
        _profileRepository = profileRepository;
        _userRepository = userRepository;
    }

    public async Task<UserProfile?> GetByUserIdAsync(int userId)
        => await _profileRepository.GetByUserIdAsync(userId);

    public async Task<(UserProfile? Profile, string? Error, bool IsConflict)> CreateAsync(int userId, CreateUserProfileDto dto)
    {
        var userExists = await _userRepository.GetByIdAsync(userId);
        if (userExists == null)
            return (null, "Usuário não encontrado.", false);

        if (await _profileRepository.ExistsForUserAsync(userId))
            return (null, "Este usuário já possui um perfil cadastrado.", true);

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

        var created = await _profileRepository.CreateAsync(profile);
        return (created, null, false);
    }

    public async Task<bool> UpdateAsync(int userId, UpdateUserProfileDto dto)
    {
        var profile = await _profileRepository.GetByUserIdAsync(userId);
        if (profile == null) return false;

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

        await _profileRepository.UpdateAsync(profile);
        return true;
    }

    public async Task<bool> DeleteAsync(int userId)
    {
        var profile = await _profileRepository.GetByUserIdAsync(userId);
        if (profile == null) return false;

        await _profileRepository.DeleteAsync(profile);
        return true;
    }
}
