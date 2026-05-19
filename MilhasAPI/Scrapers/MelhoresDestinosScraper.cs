using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using MilhasAPI.Scrapers.Models;

namespace MilhasAPI.Scrapers;

/// <summary>
/// Scraper agregador que consome o feed de promoções de milhas do MelhoresDestinos.
/// A página é SSR (HTML estático), o que permite extrair os preços por milheiro
/// publicados nos blocos de promoção sem precisar de browser headless.
///
/// Cada bloco de promoção contém o programa de destino (Smiles, Latam Pass, etc.)
/// e o preço do milheiro quando aplicável. Quando o site não cobre algum dos
/// programas conhecidos, o MilesMonitorService preenche com uma estimativa via
/// MockMilesQuoteFactory.
/// </summary>
public class MelhoresDestinosScraper : BaseMilesScraper
{
    public override string SourceName => "MelhoresDestinos";
    protected override string TargetUrl => "https://www.melhoresdestinos.com.br/milhas";

    // Mapeamento "keyword no texto do bloco" → nome canônico do programa.
    // Ordem importa: programas mais específicos primeiro (Latam Pass antes de Latam).
    private static readonly (string Keyword, string Program)[] ProgramKeywords =
    {
        ("latam pass", "Latam Pass"),
        ("smiles",     "Smiles"),
        ("tudoazul",   "TudoAzul"),
        ("azul",       "TudoAzul"),
        ("livelo",     "Livelo"),
    };

    // Captura "milheiro a partir de R$ 13,83" ou "Milheiro por R$ 15,13" — formatos
    // que o MelhoresDestinos usa nas chamadas de promoção.
    private static readonly Regex MilheiroPriceRegex = new(
        @"milheiro\s+(?:a\s+partir\s+de\s+|por\s+)?R\$\s*([\d.,]+)",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    // Captura "X% de bônus" ou "X% bônus".
    private static readonly Regex BonusRegex = new(
        @"(\d{1,3})\s*%\s*(?:de\s+)?b[ôo]nus",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    public MelhoresDestinosScraper(IHttpClientFactory httpClientFactory, ILogger<MelhoresDestinosScraper> logger)
        : base(httpClientFactory, logger) { }

    protected override async Task<IEnumerable<MilesQuote>> ExecuteScrapingAsync(CancellationToken cancellationToken)
    {
        var html = await FetchHtmlAsync(TargetUrl, cancellationToken);
        if (html is null) return Array.Empty<MilesQuote>();

        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        // Cada promoção fica em <article> ou <div class="post-promo-home-*"> — usamos
        // articles porque englobam tanto a chamada quanto o resumo.
        var blocks = doc.DocumentNode.SelectNodes("//article") ?? new HtmlNodeCollection(null);

        // program → menor preço encontrado (em R$ por milha, não por milheiro)
        var byProgram = new Dictionary<string, MilesQuote>(StringComparer.OrdinalIgnoreCase);

        foreach (var block in blocks)
        {
            var text = WebUtility.HtmlDecode(block.InnerText);
            text = Regex.Replace(text, @"\s+", " ").Trim();
            if (text.Length < 30) continue;

            var program = DetectProgram(text);
            if (program is null) continue;

            var priceMatch = MilheiroPriceRegex.Match(text);
            if (!priceMatch.Success) continue;

            if (!TryParseBrPrice(priceMatch.Groups[1].Value, out var pricePerMilheiro)) continue;
            var pricePerMile = pricePerMilheiro / 1000m;

            // Mantém a melhor (menor) cotação por programa.
            if (byProgram.TryGetValue(program, out var existing) && existing.PricePerMile <= pricePerMile)
                continue;

            var bonusMatch = BonusRegex.Match(text);
            decimal? bonusMultiplier = bonusMatch.Success && int.TryParse(bonusMatch.Groups[1].Value, out var pct)
                ? 1m + (pct / 100m)
                : null;

            var title = TrimTo(text, 160);

            byProgram[program] = new MilesQuote
            {
                Program = program,
                SourceUrl = TargetUrl,
                PricePerMile = pricePerMile,
                BonusMultiplier = bonusMultiplier,
                IsPromotion = bonusMultiplier is not null,
                PromotionDescription = title,
                ScrapedAt = DateTime.UtcNow,
            };
        }

        return byProgram.Values;
    }

    private static string? DetectProgram(string text)
    {
        var lower = text.ToLowerInvariant();
        foreach (var (keyword, program) in ProgramKeywords)
        {
            if (lower.Contains(keyword)) return program;
        }
        return null;
    }

    private static bool TryParseBrPrice(string raw, out decimal value)
    {
        // "13,83" -> 13.83 ; "1.234,56" -> 1234.56
        var normalized = raw.Replace(".", "").Replace(",", ".");
        return decimal.TryParse(normalized, NumberStyles.Any, CultureInfo.InvariantCulture, out value);
    }

    private static string TrimTo(string text, int max)
        => text.Length <= max ? text : text[..max].TrimEnd() + "…";
}
