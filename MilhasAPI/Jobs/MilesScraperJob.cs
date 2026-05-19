using MilhasAPI.Scrapers.Interfaces;

namespace MilhasAPI.Jobs;

/// <summary>
/// Background service que executa o scraping de milhas automaticamente
/// a cada intervalo configurado (padrão: 1 hora).
///
/// Roda em background sem bloquear a API.
/// Configure o intervalo em appsettings.json: "Scraper:IntervalMinutes"
/// </summary>
public class MilesScraperJob : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<MilesScraperJob> _logger;
    private readonly TimeSpan _interval;

    public MilesScraperJob(IServiceProvider serviceProvider, ILogger<MilesScraperJob> logger, IConfiguration configuration)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;

        var minutes = configuration.GetValue<int>("Scraper:IntervalMinutes", 60);
        _interval = TimeSpan.FromMinutes(minutes);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("MilesScraperJob iniciado. Intervalo: {Interval} minuto(s).", _interval.TotalMinutes);

        // Roda imediatamente na inicialização, depois aguarda o intervalo
        await RunScrapingAsync(stoppingToken);

        using var timer = new PeriodicTimer(_interval);
        while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
        {
            await RunScrapingAsync(stoppingToken);
        }
    }

    private async Task RunScrapingAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("[MilesScraperJob] Executando coleta agendada às {Time}", DateTime.UtcNow);

        // Cria um escopo para resolver serviços Scoped dentro de um Singleton (BackgroundService)
        await using var scope = _serviceProvider.CreateAsyncScope();
        var monitor = scope.ServiceProvider.GetRequiredService<IMilesMonitorService>();

        try
        {
            var quotes = await monitor.FetchAllQuotesAsync(cancellationToken);
            foreach (var quote in quotes)
            {
                _logger.LogInformation(
                    "[{Program}] R$ {Price}/milha | Promoção: {Promo}",
                    quote.Program,
                    quote.PricePerMile,
                    quote.IsPromotion ? quote.PromotionDescription ?? "Sim" : "Não");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[MilesScraperJob] Erro durante a coleta agendada.");
        }
    }
}
