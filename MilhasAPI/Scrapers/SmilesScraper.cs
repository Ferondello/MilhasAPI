using HtmlAgilityPack;
using MilhasAPI.Scrapers.Models;

namespace MilhasAPI.Scrapers;

/// <summary>
/// Scraper para o programa Smiles (GOL).
/// Coleta cotações da página de compra de milhas: https://www.smiles.com.br/compre-milhas
/// 
/// COMO ATUALIZAR OS SELETORES:
/// 1. Acesse a URL no navegador
/// 2. Inspecione o elemento que mostra o preço por milha (F12)
/// 3. Copie o seletor XPath ou CSS e atualize os campos abaixo
/// </summary>
public class SmilesScraper : BaseMilesScraper
{
    public override string ProgramName => "Smiles";
    protected override string TargetUrl => "https://www.smiles.com.br/compre-milhas";

    // ── Seletores HTML ──────────────────────────────────────────────
    // Ajuste estes XPaths se o site mudar o layout
    private const string PriceXPath = "//span[contains(@class,'price-per-mile')]";
    private const string PromoXPath = "//div[contains(@class,'promotion-banner')]";
    // ───────────────────────────────────────────────────────────────

    public SmilesScraper(IHttpClientFactory httpClientFactory, ILogger<SmilesScraper> logger)
        : base(httpClientFactory, logger) { }

    protected override async Task<MilesQuote?> ExecuteScrapingAsync(CancellationToken cancellationToken)
    {
        var html = await FetchHtmlAsync(TargetUrl, cancellationToken);
        if (html is null) return null;

        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        // Extrai o preço por milha
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

        // Verifica promoção ativa
        var promoNode = doc.DocumentNode.SelectSingleNode(PromoXPath);
        var hasPromo = promoNode is not null;

        return new MilesQuote
        {
            Program = ProgramName,
            SourceUrl = TargetUrl,
            PricePerMile = price,
            IsPromotion = hasPromo,
            PromotionDescription = hasPromo ? promoNode!.InnerText.Trim() : null,
            ScrapedAt = DateTime.UtcNow
        };
    }

    private static bool TryParsePrice(string rawText, out decimal price)
    {
        price = 0;
        // Remove "R$", espaços e trata vírgula decimal (padrão BR)
        var cleaned = rawText
            .Replace("R$", "")
            .Replace(".", "")
            .Replace(",", ".")
            .Trim();

        return decimal.TryParse(cleaned, System.Globalization.NumberStyles.Any,
            System.Globalization.CultureInfo.InvariantCulture, out price);
    }
}
