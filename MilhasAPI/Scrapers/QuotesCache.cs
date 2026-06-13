using MilhasAPI.Scrapers.Interfaces;
using MilhasAPI.Scrapers.Models;

namespace MilhasAPI.Scrapers;

/// <summary>
/// Implementação em memória, thread-safe, do cache de cotações.
/// Substituível por uma implementação distribuída (Redis, etc.) sem tocar
/// no resto do sistema, já que os consumidores dependem de <see cref="IQuotesCache"/>.
/// </summary>
public class QuotesCache : IQuotesCache
{
    private readonly object _gate = new();
    private List<MilesQuote> _quotes = new();

    public void Replace(IEnumerable<MilesQuote> quotes)
    {
        var snapshot = quotes.ToList();
        lock (_gate)
        {
            _quotes = snapshot;
        }
    }

    public IReadOnlyList<MilesQuote> GetAll()
    {
        lock (_gate)
        {
            return _quotes.ToList();
        }
    }
}
