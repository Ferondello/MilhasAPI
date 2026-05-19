namespace MilhasAPI.Scrapers.Models;

/// <summary>
/// Representa a cotação de milhas de um programa de fidelidade em um momento específico.
/// </summary>
public class MilesQuote
{
    public string Program { get; set; } = null!;        // Ex: "Smiles", "Latam Pass"
    public string SourceUrl { get; set; } = null!;      // URL de onde veio o dado
    public decimal PricePerMile { get; set; }           // Preço por milha em R$
    public decimal? BonusMultiplier { get; set; }       // Multiplicador de bônus (ex: 1.5 = 50% bônus)
    public string? PromotionDescription { get; set; }   // Descrição da promoção ativa
    public DateTime ScrapedAt { get; set; } = DateTime.UtcNow;
    public bool IsPromotion { get; set; }
}
