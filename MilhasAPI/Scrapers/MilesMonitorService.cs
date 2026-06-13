using MilhasAPI.Scrapers.Interfaces;
using MilhasAPI.Scrapers.Models;

namespace MilhasAPI.Scrapers;

/// <summary>
/// Orquestra todos os scrapers registrados via injeção de dependência e consolida
/// os resultados num único conjunto de cotações por programa. Quando um programa
/// esperado pela UI não tem dado real no ciclo, gera uma estimativa via
/// <see cref="IMilesQuoteEstimator"/> pra manter a tela sempre populada.
///
/// O resultado consolidado é guardado no <see cref="IQuotesCache"/> (singleton),
/// de onde as requisições HTTP leem sem disparar nova coleta.
/// </summary>
public class MilesMonitorService : IMilesMonitorService
{
    /// <summary>
    /// Programas que a UI espera sempre exibir. Se um scraper não trouxer dado real
    /// pra algum deles, completamos com estimativa.
    /// </summary>
    private static readonly string[] ExpectedPrograms =
        { "Smiles", "Latam Pass", "Livelo", "TudoAzul" };

    private readonly IEnumerable<IMilesScraper> _scrapers;
    private readonly IMilesQuoteEstimator _estimator;
    private readonly IQuotesCache _cache;
    private readonly ILogger<MilesMonitorService> _logger;

    public MilesMonitorService(
        IEnumerable<IMilesScraper> scrapers,
        IMilesQuoteEstimator estimator,
        IQuotesCache cache,
        ILogger<MilesMonitorService> logger)
    {
        _scrapers = scrapers;
        _estimator = estimator;
        _cache = cache;
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

        // Preenche estimativa para programas esperados que ficaram fora.
        foreach (var program in ExpectedPrograms)
        {
            if (!byProgram.ContainsKey(program))
            {
                byProgram[program] = _estimator.GenerateFor(program);
                _logger.LogInformation("[{Program}] sem dado real — usando estimativa.", program);
            }
        }

        var consolidated = byProgram.Values
            .OrderBy(q => q.PricePerMile)
            .ToList();

        _cache.Replace(consolidated);

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
        => Task.FromResult<IEnumerable<MilesQuote>>(_cache.GetAll());
}
