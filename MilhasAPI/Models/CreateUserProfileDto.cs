namespace MilhasAPI.Models;

public class CreateUserProfileDto
{
    // Dados financeiros
    public decimal MonthlyIncome { get; set; }
    public InvestmentProfile InvestmentProfile { get; set; }
    public decimal MonthlyCardSpending { get; set; }
    public decimal AnnualCardFeeBudget { get; set; }
    public int NumberOfCreditCards { get; set; }

    // Dados de viagem e milhas
    public TravelFrequency TravelFrequency { get; set; }
    public CabinClass PreferredCabinClass { get; set; }
    public LoyaltyProgram PreferredLoyaltyProgram { get; set; }
    public int CurrentMilesBalance { get; set; }
    public int MonthlyMilesGoal { get; set; }

    // Preferências de destino
    public bool PrefersDomesticTravel { get; set; }
    public bool PrefersInternationalTravel { get; set; }

    // Dados para análise de ofertas
    public decimal MaxMilePurchasePrice { get; set; }
    public bool InterestedInCardUpgrades { get; set; }
    public bool InterestedInMilesTransferPromos { get; set; }
}
