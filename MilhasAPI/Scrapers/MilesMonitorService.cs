using MilhasAPI.Scrapers.Interfaces;
using MilhasAPI.Scrapers.Models;

namespace MilhasAPI.Scrapers;

/// <summary>
/// Orquestra todos os scrapers registrados via injeção de dependência e consolida
/// os resultados num único conjunto de cotações por programa. Quando um programa
/// esperado pela UI não tem dado real no ciclo, gera uma estimativa via
/// <see cref="MockMilesQuoteFactory"/> pra manter a tela sempre populada.
/// </summary>
public class MilesMonitorService : IMilesMonitorService
{
    /// <summary>
    /// Programas que a UI espera sempre exibir. Se um scraper não trouxer dado real
    /// pra algum deles, completamos com mock.
    /// </summary>
    private static readonly string[] ExpectedPrograms =
        { "Smiles", "Latam Pass", "Livelo", "TudoAzul" };

    private readonly IEnumerable<IMilesScraper> _scrapers;
    private readonly ILogger<MilesMonitorService> _logger;

    private readonly List<MilesQuote> _latestQuotes = new();
    private readonly SemaphoreSlim _lock = new(1, 1);

    public MilesMonitorService(IEnumerable<IMilesScraper> scrapers, ILogger<MilesMonitorService> logger)
    {
        _scrapers = scrapers;
        _logger = logger;
    }

    public async Task<IEnumerable<MilesQuote>> FetchAllQuotesAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Iniciando coleta em {Count} fonte(s).", _scrapers.Count());

        var tasks = _scrapers.Select(s => s.ScrapeAsync(cancellationToken));
        var resultsByScraper = await Task.WhenAll(tasks);
        var allReal = resultsByScraper.SelectMany(r => r);

        // Para cada programa, mantém apenas a melhor (menor) cotação real.
        var byProgram = allReal
            .GroupBy(q => q.Program, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(g => g.Key, g => g.OrderBy(q => q.PricePerMile).First(),
                          StringComparer.OrdinalIgnoreCase);

        // Preenche mock para programas esperados que ficaram fora.
        foreach (var program in ExpectedPrograms)
        {
            if (!byProgram.ContainsKey(program))
            {
                byProgram[program] = MockMilesQuoteFactory.GenerateFor(program);
                _logger.LogInformation("[{Program}] sem dado real — usando estimativa.", program);
            }
        }

        var consolidated = byProgram.Values
            .OrderBy(q => q.PricePerMile)
            .ToList();

        await _lock.WaitAsync(cancellationToken);
        try
        {
            _latestQuotes.Clear();
            _latestQuotes.AddRange(consolidated);
        }
        finally
        {
            _lock.Release();
        }

        _logger.LogInformation("Coleta finalizada. {Count} cotação(ões) no cache.", consolidated.Count);
        return consolidated;
    }

    public async Task<MilesQuote?> FetchQuoteAsync(string programName, CancellationToken cancellationToken = default)
    {
        // Roda toda a coleta e devolve apenas o programa solicitado. Mais simples e
        // garante consistência com o cache. Se o programa for desconhecido, retorna null.
        var all = await FetchAllQuotesAsync(cancellationToken);
        return all.FirstOrDefault(q => q.Program.Equals(programName, StringComparison.OrdinalIgnoreCase));
    }

    public Task<IEnumerable<MilesQuote>> GetLatestQuotesAsync()
        => Task.FromResult<IEnumerable<MilesQuote>>(_latestQuotes.ToList());
}
