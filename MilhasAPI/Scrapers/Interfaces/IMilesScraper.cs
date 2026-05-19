using MilhasAPI.Scrapers.Models;

namespace MilhasAPI.Scrapers.Interfaces;

/// <summary>
/// Contrato para qualquer scraper de cotações de milhas.
/// Um scraper pode cobrir múltiplos programas a partir de uma mesma fonte
/// (ex: um agregador de notícias que publica preços de Smiles, Latam, etc.).
/// </summary>
public interface IMilesScraper
{
    /// <summary>Nome/identificação da fonte (usado em logs).</summary>
    string SourceName { get; }

    /// <summary>Busca as cotações disponíveis na fonte. Retorna lista vazia em caso de falha.</summary>
    Task<IEnumerable<MilesQuote>> ScrapeAsync(CancellationToken cancellationToken = default);
}
