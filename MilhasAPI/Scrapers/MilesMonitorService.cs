using MilhasAPI.Scrapers.Interfaces;
using MilhasAPI.Scrapers.Models;

namespace MilhasAPI.Scrapers;

/// <summary>
/// Orquestra todos os scrapers registrados via injeção de dependência.
/// Para adicionar um novo programa, basta registrar o scraper no Program.cs —
/// este serviço o descobrirá automaticamente sem precisar ser alterado. (Open/Closed)
/// </summary>
public class MilesMonitorService : IMilesMonitorService
{
    private readonly IEnumerable<IMilesScraper> _scrapers;
    private readonly ILogger<MilesMonitorService> _logger;

    // Cache em memória das últimas cotações coletadas
    private readonly List<MilesQuote> _latestQuotes = new();
    private readonly SemaphoreSlim _lock = new(1, 1);

    public MilesMonitorService(IEnumerable<IMilesScraper> scrapers, ILogger<MilesMonitorService> logger)
    {
        _scrapers = scrapers;
        _logger = logger;
    }

    public async Task<IEnumerable<MilesQuote>> FetchAllQuotesAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Iniciando coleta de cotações para {Count} programa(s).", _scrapers.Count());

        // Roda todos os scrapers em paralelo
        var tasks = _scrapers.Select(s => s.ScrapeAsync(cancellationToken));
        var results = await Task.WhenAll(tasks);

        var collected = results.Where(q => q is not null).Cast<MilesQuote>().ToList();

        await _lock.WaitAsync(cancellationToken);
        try
        {
            _latestQuotes.Clear();
            _latestQuotes.AddRange(collected);
        }
        finally
        {
            _lock.Release();
        }

        _logger.LogInformation("Coleta finalizada. {Success}/{Total} programas responderam.", collected.Count, _scrapers.Count());
        return collected;
    }

    public async Task<MilesQuote?> FetchQuoteAsync(string programName, CancellationToken cancellationToken = default)
    {
        var scraper = _scrapers.FirstOrDefault(s =>
            s.ProgramName.Equals(programName, StringComparison.OrdinalIgnoreCase));

        if (scraper is null)
        {
            _logger.LogWarning("Scraper não encontrado para o programa: {Program}", programName);
            return null;
        }

        return await scraper.ScrapeAsync(cancellationToken);
    }

    public Task<IEnumerable<MilesQuote>> GetLatestQuotesAsync()
        => Task.FromResult<IEnumerable<MilesQuote>>(_latestQuotes.ToList());
}
