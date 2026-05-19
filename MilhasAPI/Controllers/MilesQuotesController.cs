using Microsoft.AspNetCore.Mvc;
using MilhasAPI.Scrapers.Interfaces;
using MilhasAPI.Scrapers.Models;

namespace MilhasAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MilesQuotesController : ControllerBase
{
    private readonly IMilesMonitorService _monitorService;

    public MilesQuotesController(IMilesMonitorService monitorService)
    {
        _monitorService = monitorService;
    }

    /// <summary>
    /// Retorna as cotações já coletadas (cache em memória, sem novo scraping).
    /// Use este endpoint para consultas frequentes do frontend.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MilesQuote>>> GetLatest()
    {
        var quotes = await _monitorService.GetLatestQuotesAsync();
        return Ok(quotes);
    }

    /// <summary>
    /// Força uma nova coleta em todos os programas agora (pode demorar alguns segundos).
    /// </summary>
    [HttpPost("refresh")]
    public async Task<ActionResult<IEnumerable<MilesQuote>>> RefreshAll(CancellationToken cancellationToken)
    {
        var quotes = await _monitorService.FetchAllQuotesAsync(cancellationToken);
        return Ok(quotes);
    }

    /// <summary>
    /// Força uma nova coleta apenas para um programa específico.
    /// Ex: GET /api/milesquotes/refresh/Smiles
    /// </summary>
    [HttpPost("refresh/{programName}")]
    public async Task<ActionResult<MilesQuote>> RefreshOne(string programName, CancellationToken cancellationToken)
    {
        var quote = await _monitorService.FetchQuoteAsync(programName, cancellationToken);
        if (quote is null)
            return NotFound($"Programa '{programName}' não encontrado ou falhou ao coletar dados.");

        return Ok(quote);
    }
}
