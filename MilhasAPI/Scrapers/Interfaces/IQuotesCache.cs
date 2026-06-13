using MilhasAPI.Scrapers.Models;

namespace MilhasAPI.Scrapers.Interfaces;

/// <summary>
/// Armazena a última coleta consolidada de cotações em memória.
/// Vive como singleton para que o resultado preenchido pelo job em background
/// fique disponível para as requisições HTTP subsequentes (escopos diferentes).
/// </summary>
public interface IQuotesCache
{
    void Replace(IEnumerable<MilesQuote> quotes);
    IReadOnlyList<MilesQuote> GetAll();
}
