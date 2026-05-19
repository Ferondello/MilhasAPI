using MilhasAPI.Scrapers.Models;

namespace MilhasAPI.Scrapers.Interfaces;

/// <summary>
/// Contrato para qualquer scraper de programa de milhas.
/// Cada implementação é responsável por um único programa (Smiles, Latam, Livelo...).
/// </summary>
public interface IMilesScraper
{
    /// <summary>Nome do programa de milhas que este scraper atende.</summary>
    string ProgramName { get; }

    /// <summary>Busca a cotação atual de milhas do programa.</summary>
    Task<MilesQuote?> ScrapeAsync(CancellationToken cancellationToken = default);
}
