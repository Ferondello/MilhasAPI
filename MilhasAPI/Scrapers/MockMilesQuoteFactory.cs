using MilhasAPI.Scrapers.Models;

namespace MilhasAPI.Scrapers;

/// <summary>
/// Gera cotações estimadas para programas que não foram cobertos pela coleta real
/// no ciclo atual. As faixas refletem preços de mercado observados em 2025–2026
/// (em R$ por milheiro). O frontend exibe a marcação "Estimativa" no campo
/// <see cref="MilesQuote.PromotionDescription"/>.
/// </summary>
public static class MockMilesQuoteFactory
{
    private static readonly Random _random = new();

    // Faixas em R$ por milheiro (1.000 milhas). São divididas por 1000 antes de gravar.
    private static readonly Dictionary<string, (decimal Min, decimal Max)> Ranges =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["Smiles"]     = (14.00m, 18.00m),
            ["Latam Pass"] = (18.00m, 25.00m),
            ["Livelo"]     = (15.00m, 19.00m),
            ["TudoAzul"]   = (13.00m, 17.00m),
        };

    // URLs oficiais dos programas — usadas no botão "Ver site" da UI quando o dado é estimado.
    private static readonly Dictionary<string, string> ProgramUrls =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["Smiles"]     = "https://www.smiles.com.br/compre-milhas",
            ["Latam Pass"] = "https://latampass.latam.com",
            ["Livelo"]     = "https://www.livelo.com.br",
            ["TudoAzul"]   = "https://tudoazul.voeazul.com.br",
        };

    public static MilesQuote GenerateFor(string program)
    {
        var range = Ranges.TryGetValue(program, out var r) ? r : (Min: 15.00m, Max: 25.00m);
        var pricePerMilheiro = range.Min + (decimal)_random.NextDouble() * (range.Max - range.Min);
        var pricePerMile = Math.Round(pricePerMilheiro / 1000m, 5);
        var url = ProgramUrls.TryGetValue(program, out var u) ? u : "https://www.melhoresdestinos.com.br/milhas";

        return new MilesQuote
        {
            Program = program,
            SourceUrl = url,
            PricePerMile = pricePerMile,
            BonusMultiplier = null,
            IsPromotion = false,
            PromotionDescription = "Estimativa (fonte indisponível no momento)",
            ScrapedAt = DateTime.UtcNow,
        };
    }
}
