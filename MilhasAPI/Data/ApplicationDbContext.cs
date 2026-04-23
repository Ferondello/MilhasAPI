using Microsoft.EntityFrameworkCore;
using MilhasAPI.Models;

namespace MilhasAPI.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<CreditCard> CreditCards { get; set; }
    public DbSet<RewardTransaction> RewardTransactions { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasOne(u => u.Profile)
            .WithOne(p => p.User)
            .HasForeignKey<UserProfile>(p => p.UserId);

        // Seed de usuários
        modelBuilder.Entity<User>().HasData(
            new { Id = 100, Name = "Ana Silva", Email = "ana.silva@email.com" },
            new { Id = 101, Name = "Carlos Mendes", Email = "carlos.mendes@email.com" },
            new { Id = 102, Name = "Beatriz Oliveira", Email = "beatriz.oliveira@email.com" },
            new { Id = 103, Name = "Rafael Costa", Email = "rafael.costa@email.com" },
            new { Id = 104, Name = "Juliana Santos", Email = "juliana.santos@email.com" }
        );

        // Seed de cartões de crédito
        modelBuilder.Entity<CreditCard>().HasData(
            new { Id = 100, CardNumber = "**** **** **** 1234", Brand = "Visa Infinite", UserId = 100 },
            new { Id = 101, CardNumber = "**** **** **** 5678", Brand = "Mastercard Black", UserId = 100 },
            new { Id = 102, CardNumber = "**** **** **** 9012", Brand = "Amex Platinum", UserId = 101 },
            new { Id = 103, CardNumber = "**** **** **** 3456", Brand = "Visa Platinum", UserId = 101 },
            new { Id = 104, CardNumber = "**** **** **** 7890", Brand = "Elo Nanquim", UserId = 102 },
            new { Id = 105, CardNumber = "**** **** **** 2345", Brand = "Mastercard Platinum", UserId = 102 },
            new { Id = 106, CardNumber = "**** **** **** 6789", Brand = "Visa Gold", UserId = 103 },
            new { Id = 107, CardNumber = "**** **** **** 0123", Brand = "Amex Gold", UserId = 104 },
            new { Id = 108, CardNumber = "**** **** **** 4567", Brand = "Mastercard Black", UserId = 104 }
        );

        // Seed de perfis
        modelBuilder.Entity<UserProfile>().HasData(
            new
            {
                Id = 100, UserId = 100,
                MonthlyIncome = 12000m,
                InvestmentProfile = InvestmentProfile.Aggressive,
                MonthlyCardSpending = 8000m,
                AnnualCardFeeBudget = 1500m,
                NumberOfCreditCards = 2,
                TravelFrequency = TravelFrequency.Frequent,
                PreferredCabinClass = CabinClass.Business,
                PreferredLoyaltyProgram = LoyaltyProgram.Livelo,
                CurrentMilesBalance = 120000,
                MonthlyMilesGoal = 15000,
                PrefersDomesticTravel = true,
                PrefersInternationalTravel = true,
                MaxMilePurchasePrice = 22m,
                InterestedInCardUpgrades = true,
                InterestedInMilesTransferPromos = true
            },
            new
            {
                Id = 101, UserId = 101,
                MonthlyIncome = 25000m,
                InvestmentProfile = InvestmentProfile.Aggressive,
                MonthlyCardSpending = 18000m,
                AnnualCardFeeBudget = 3000m,
                NumberOfCreditCards = 2,
                TravelFrequency = TravelFrequency.VeryFrequent,
                PreferredCabinClass = CabinClass.FirstClass,
                PreferredLoyaltyProgram = LoyaltyProgram.Smiles,
                CurrentMilesBalance = 350000,
                MonthlyMilesGoal = 30000,
                PrefersDomesticTravel = false,
                PrefersInternationalTravel = true,
                MaxMilePurchasePrice = 30m,
                InterestedInCardUpgrades = true,
                InterestedInMilesTransferPromos = true
            },
            new
            {
                Id = 102, UserId = 102,
                MonthlyIncome = 7000m,
                InvestmentProfile = InvestmentProfile.Moderate,
                MonthlyCardSpending = 4000m,
                AnnualCardFeeBudget = 600m,
                NumberOfCreditCards = 2,
                TravelFrequency = TravelFrequency.Occasional,
                PreferredCabinClass = CabinClass.PremiumEconomy,
                PreferredLoyaltyProgram = LoyaltyProgram.Azul,
                CurrentMilesBalance = 45000,
                MonthlyMilesGoal = 8000,
                PrefersDomesticTravel = true,
                PrefersInternationalTravel = false,
                MaxMilePurchasePrice = 18m,
                InterestedInCardUpgrades = false,
                InterestedInMilesTransferPromos = true
            },
            new
            {
                Id = 103, UserId = 103,
                MonthlyIncome = 4500m,
                InvestmentProfile = InvestmentProfile.Conservative,
                MonthlyCardSpending = 2500m,
                AnnualCardFeeBudget = 0m,
                NumberOfCreditCards = 1,
                TravelFrequency = TravelFrequency.Rarely,
                PreferredCabinClass = CabinClass.Economy,
                PreferredLoyaltyProgram = LoyaltyProgram.Latam,
                CurrentMilesBalance = 8000,
                MonthlyMilesGoal = 3000,
                PrefersDomesticTravel = true,
                PrefersInternationalTravel = false,
                MaxMilePurchasePrice = 12m,
                InterestedInCardUpgrades = false,
                InterestedInMilesTransferPromos = false
            },
            new
            {
                Id = 104, UserId = 104,
                MonthlyIncome = 15000m,
                InvestmentProfile = InvestmentProfile.Moderate,
                MonthlyCardSpending = 10000m,
                AnnualCardFeeBudget = 2000m,
                NumberOfCreditCards = 2,
                TravelFrequency = TravelFrequency.Frequent,
                PreferredCabinClass = CabinClass.Business,
                PreferredLoyaltyProgram = LoyaltyProgram.Esfera,
                CurrentMilesBalance = 200000,
                MonthlyMilesGoal = 20000,
                PrefersDomesticTravel = true,
                PrefersInternationalTravel = true,
                MaxMilePurchasePrice = 25m,
                InterestedInCardUpgrades = true,
                InterestedInMilesTransferPromos = true
            }
        );

        // Seed de transações de recompensa
        modelBuilder.Entity<RewardTransaction>().HasData(
            new { Id = 100, UserId = 100, CreditCardId = 100, Date = new DateTime(2026, 1, 15, 0, 0, 0, DateTimeKind.Utc), Amount = 2500m, MilesEarned = 7500 },
            new { Id = 101, UserId = 100, CreditCardId = 101, Date = new DateTime(2026, 2, 10, 0, 0, 0, DateTimeKind.Utc), Amount = 3200m, MilesEarned = 9600 },
            new { Id = 102, UserId = 101, CreditCardId = 102, Date = new DateTime(2026, 1, 20, 0, 0, 0, DateTimeKind.Utc), Amount = 8000m, MilesEarned = 24000 },
            new { Id = 103, UserId = 101, CreditCardId = 103, Date = new DateTime(2026, 3, 5, 0, 0, 0, DateTimeKind.Utc), Amount = 5500m, MilesEarned = 11000 },
            new { Id = 104, UserId = 102, CreditCardId = 104, Date = new DateTime(2026, 2, 28, 0, 0, 0, DateTimeKind.Utc), Amount = 1800m, MilesEarned = 3600 },
            new { Id = 105, UserId = 103, CreditCardId = 106, Date = new DateTime(2026, 3, 12, 0, 0, 0, DateTimeKind.Utc), Amount = 900m, MilesEarned = 1800 },
            new { Id = 106, UserId = 104, CreditCardId = 107, Date = new DateTime(2026, 1, 8, 0, 0, 0, DateTimeKind.Utc), Amount = 4200m, MilesEarned = 12600 },
            new { Id = 107, UserId = 104, CreditCardId = 108, Date = new DateTime(2026, 2, 18, 0, 0, 0, DateTimeKind.Utc), Amount = 6100m, MilesEarned = 18300 }
        );
    }
}
