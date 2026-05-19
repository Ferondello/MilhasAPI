using HtmlAgilityPack;
using MilhasAPI.Scrapers.Models;

namespace MilhasAPI.Scrapers;

/// <summary>
/// Scraper para o programa Livelo.
/// Coleta cotações da página de compra de pontos: https://www.livelo.com.br/compre-pontos
///
/// COMO ATUALIZAR OS SELETORES:
/// 1. Acesse a URL no navegador
/// 2. Inspecione o elemento que mostra o preço por ponto (F12)
/// 3. Copie o seletor XPath e atualize as constantes abaixo
/// </summary>
public class LiveloScraper : BaseMilesScraper
{
    public override string ProgramName => "Livelo";
    protected override string TargetUrl => "https://www.livelo.com.br/compre-pontos";

    // ── Seletores HTML ──────────────────────────────────────────────
    private const string PriceXPath = "//span[contains(@class,'value-per-point')]";
    private const string PromoXPath = "//div[contains(@class,'offer-banner')]";
    private const string BonusXPath = "//span[contains(@class,'bonus-tag')]";
    // ───────────────────────────────────────────────────────────────

    public LiveloScraper(IHttpClientFactory httpClientFactory, ILogger<LiveloScraper> logger)
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

        var promoNode = doc.DocumentNode.SelectSingleNode(PromoXPath);
        var bonusNode = doc.DocumentNode.SelectSingleNode(BonusXPath);

        decimal? bonusMultiplier = null;
        if (bonusNode is not null && TryParseBonus(bonusNode.InnerText, out var bonus))
            bonusMultiplier = bonus;

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
        var cleaned = rawText.Replace("%", "").Trim();
        if (!decimal.TryParse(cleaned, out var percent)) return false;
        multiplier = 1 + (percent / 100);
        return true;
    }
}
