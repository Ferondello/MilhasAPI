using MilhasAPI.Scrapers.Models;

namespace MilhasAPI.Scrapers.Interfaces;

/// <summary>
/// Orquestra todos os scrapers registrados e consolida os resultados.
/// </summary>
public interface IMilesMonitorService
{
    /// <summary>Executa todos os scrapers e retorna as cotações coletadas.</summary>
    Task<IEnumerable<MilesQuote>> FetchAllQuotesAsync(CancellationToken cancellationToken = default);

    /// <summary>Executa apenas o scraper de um programa específico.</summary>
    Task<MilesQuote?> FetchQuoteAsync(string programName, CancellationToken cancellationToken = default);

    /// <summary>Retorna as cotações mais recentes já armazenadas no banco.</summary>
    Task<IEnumerable<MilesQuote>> GetLatestQuotesAsync();
}
