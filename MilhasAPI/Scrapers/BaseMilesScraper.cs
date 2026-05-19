using MilhasAPI.Scrapers.Interfaces;
using MilhasAPI.Scrapers.Models;

namespace MilhasAPI.Scrapers;

public abstract class BaseMilesScraper : IMilesScraper
{
    protected readonly HttpClient _httpClient;
    protected readonly ILogger _logger;

    public abstract string SourceName { get; }
    protected abstract string TargetUrl { get; }

    protected BaseMilesScraper(IHttpClientFactory httpClientFactory, ILogger logger)
    {
        _httpClient = httpClientFactory.CreateClient("ScraperClient");
        _logger = logger;
    }

    public async Task<IEnumerable<MilesQuote>> ScrapeAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("[{Source}] Iniciando scraping em {Url}", SourceName, TargetUrl);
            var quotes = (await ExecuteScrapingAsync(cancellationToken)).ToList();
            _logger.LogInformation("[{Source}] {Count} cotação(ões) coletada(s).", SourceName, quotes.Count);
            return quotes;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "[{Source}] Falha na requisição HTTP para {Url}", SourceName, TargetUrl);
            return Array.Empty<MilesQuote>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[{Source}] Erro inesperado durante o scraping", SourceName);
            return Array.Empty<MilesQuote>();
        }
    }

    protected abstract Task<IEnumerable<MilesQuote>> ExecuteScrapingAsync(CancellationToken cancellationToken);

    protected async Task<string?> FetchHtmlAsync(string url, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync(url, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync(cancellationToken);
    }
}
