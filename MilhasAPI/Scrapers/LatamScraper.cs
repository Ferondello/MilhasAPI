using HtmlAgilityPack;
using MilhasAPI.Scrapers.Models;

namespace MilhasAPI.Scrapers;

/// <summary>
/// Scraper para o programa LATAM Pass.
/// Coleta cotações da página de compra de pontos: https://www.latampass.latam.com/pt_br/compre-pontos
///
/// COMO ATUALIZAR OS SELETORES:
/// 1. Acesse a URL no navegador
/// 2. Inspecione o elemento que mostra o preço por ponto (F12)
/// 3. Copie o seletor XPath e atualize as constantes abaixo
/// </summary>
public class LatamScraper : BaseMilesScraper
{
    public override string ProgramName => "Latam Pass";
    protected override string TargetUrl => "https://www.latampass.latam.com/pt_br/compre-pontos";

    // ── Seletores HTML ──────────────────────────────────────────────
    private const string PriceXPath = "//span[contains(@class,'points-price')]";
    private const string BonusXPath = "//span[contains(@class,'bonus-percentage')]";
    private const string PromoXPath = "//div[contains(@class,'promo-tag')]";
    // ───────────────────────────────────────────────────────────────

    public LatamScraper(IHttpClientFactory httpClientFactory, ILogger<LatamScraper> logger)
        : base(httpClientFactory, logger) { }

    protected override async Task<MilesQuote?> ExecuteScrapingAsync(CancellationToken cancellationToken)
    {
        var html = await FetchHtmlAsync(TargetUrl, cancellationToken);
        if (html is null) return null;

        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        var priceNode = doc.DocumentNode.SelectSingleNode(PriceXPath);
        if (priceNode is null)
        {
            _logger.LogWarning("[{Program}] Nó de preço não encontrado. O layout do site pode ter mudado.", ProgramName);
            return null;
        }

        if (!TryParsePrice(priceNode.InnerText, out var price))
        {
            _logger.LogWarning("[{Program}] Não foi possível converter o preço: '{Text}'", ProgramName, priceNode.InnerText);
            return null;
        }

        // Bônus de pontos (ex: "50% de bônus" → multiplier 1.5)
        decimal? bonusMultiplier = null;
        var bonusNode = doc.DocumentNode.SelectSingleNode(BonusXPath);
        if (bonusNode is not null && TryParseBonus(bonusNode.InnerText, out var bonus))
            bonusMultiplier = bonus;

        var promoNode = doc.DocumentNode.SelectSingleNode(PromoXPath);

        return new MilesQuote
        {
            Program = ProgramName,
            SourceUrl = TargetUrl,
            PricePerMile = price,
            BonusMultiplier = bonusMultiplier,
            IsPromotion = promoNode is not null || bonusMultiplier.HasValue,
            PromotionDescription = promoNode?.InnerText.Trim(),
            ScrapedAt = DateTime.UtcNow
        };
    }

    private static bool TryParsePrice(string rawText, out decimal price)
    {
        price = 0;
        var cleaned = rawText
            .Replace("R$", "")
            .Replace(".", "")
            .Replace(",", ".")
            .Trim();
        return decimal.TryParse(cleaned, System.Globalization.NumberStyles.Any,
            System.Globalization.CultureInfo.InvariantCulture, out price);
    }

    private static bool TryParseBonus(string rawText, out decimal multiplier)
    {
        multiplier = 1;
        // Ex: "50%" → 1.5
        var cleaned = rawText.Replace("%", "").Trim();
        if (!decimal.TryParse(cleaned, out var percent)) return false;
        multiplier = 1 + (percent / 100);
        return true;
    }
}
