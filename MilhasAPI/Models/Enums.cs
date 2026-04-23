namespace MilhasAPI.Models;

public enum InvestmentProfile
{
    Conservative,
    Moderate,
    Aggressive
}

public enum TravelFrequency
{
    Rarely,        // 0-1 viagens/ano
    Occasional,    // 2-4 viagens/ano
    Frequent,      // 5-10 viagens/ano
    VeryFrequent   // 10+ viagens/ano
}

public enum CabinClass
{
    Economy,
    PremiumEconomy,
    Business,
    FirstClass
}

public enum LoyaltyProgram
{
    Smiles,
    Latam,
    Azul,
    Multiplus,
    Livelo,
    Esfera,
    Other
}
