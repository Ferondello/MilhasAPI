using Microsoft.AspNetCore.Mvc;
using MilhasAPI.Services.Interfaces;

namespace MilhasAPI.Controllers;

/// <summary>
/// Expõe os endpoints do dashboard. A agregação e o mapeamento visual ficam no
/// <see cref="IDashboardService"/> — o controller apenas traduz HTTP.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    /// <summary>GET /api/dashboard/resumo</summary>
    [HttpGet("resumo")]
    public async Task<IActionResult> GetResumo()
        => Ok(await _dashboardService.GetResumoAsync());

    /// <summary>GET /api/dashboard/analytics</summary>
    [HttpGet("analytics")]
    public async Task<IActionResult> GetAnalytics()
        => Ok(await _dashboardService.GetAnalyticsAsync());
}
