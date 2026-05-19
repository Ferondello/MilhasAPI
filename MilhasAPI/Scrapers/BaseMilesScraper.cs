using MilhasAPI.Scrapers.Interfaces;
using MilhasAPI.Scrapers.Models;

namespace MilhasAPI.Scrapers;

/// <summary>
/// Classe base com comportamento comum a todos os scrapers:
/// logging, tratamento de erros e retry simples.
/// As subclasses implementam apenas a lógica específica de cada site.
/// </summary>
public abstract class BaseMilesScraper : IMilesScraper
{
    protected readonly HttpClient _httpClient;
    protected readonly ILogger _logger;

    public abstract string ProgramName { get; }
    protected abstract string TargetUrl { get; }

    protected BaseMilesScraper(IHttpClientFactory httpClientFactory, ILogger logger)
    {
        _httpClient = httpClientFactory.CreateClient("ScraperClient");
        _logger = logger;
    }

    public async Task<MilesQuote?> ScrapeAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("[{Program}] Iniciando scraping em {Url}", ProgramName, TargetUrl);
            var quote = await ExecuteScrapingAsync(cancellationToken);

            if (quote != null)
                _logger.LogInformation("[{Program}] Cotação coletada: R$ {Price}/milha", ProgramName, quote.PricePerMile);

            return quote;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "[{Program}] Falha na requisição HTTP para {Url}", ProgramName, TargetUrl);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[{Program}] Erro inesperado durante o scraping", ProgramName);
            return null;
        }
    }

    /// <summary>
    /// Implementação específica de cada scraper.
    /// Deve retornar null se não conseguir extrair os dados.
    /// </summary>
    protected abstract Task<MilesQuote?> ExecuteScrapingAsync(CancellationToken cancellationToken);

    /// <summary>Faz o download do HTML de uma URL.</summary>
    protected async Task<string?> FetchHtmlAsync(string url, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync(url, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync(cancellationToken);
    }
}
