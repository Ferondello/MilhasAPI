using MilhasAPI.Models;
using MilhasAPI.Services.Interfaces;
using MilhasAPI.Utils;

namespace MilhasAPI.Services;

public class DashboardService : IDashboardService
{
    private readonly IUserService _userService;
    private readonly IRewardTransactionService _transactionService;

    public DashboardService(IUserService userService, IRewardTransactionService transactionService)
    {
        _userService = userService;
        _transactionService = transactionService;
    }

    public async Task<DashboardResumoDto> GetResumoAsync()
    {
        var users = await _userService.GetAllAsync();
        var transactions = await _transactionService.GetAllAsync();

        var withProfile = users.Where(u => u.Profile != null).ToList();

        var totalMiles = withProfile.Sum(u => u.Profile!.CurrentMilesBalance);

        var programas = withProfile
            .GroupBy(u => u.Profile!.PreferredLoyaltyProgram.ToString())
            .Select((g, i) => new ProgramaResumoDto
            {
                Id = (i + 1).ToString(),
                Name = g.Key,
                Miles = g.Sum(u => u.Profile!.CurrentMilesBalance),
                Color = ProgramVisuals.Color(g.Key),
                Logo = ProgramVisuals.Logo(g.Key),
            })
            .ToList();

        var transacoes = transactions
            .OrderByDescending(t => t.Date)
            .Take(10)
            .Select(t => new TransacaoDto
            {
                Id = t.Id.ToString(),
                Type = "credit",
                Program = t.CreditCard?.Brand ?? "Cartão",
                Amount = t.MilesEarned,
                Date = t.Date.ToString("dd/MM/yyyy"),
                Description = $"Compra R$ {t.Amount:N2}",
            })
            .ToList();

        var alerts = new List<AlertDto>
        {
            new() { Id = "1", Text = "Verifique as cotações — há promoções ativas!", Type = "info" },
        };

        return new DashboardResumoDto
        {
            TotalMiles = totalMiles,
            Crescimento = 4000,
            AVencer = 5000,
            Programas = programas,
            Transacoes = transacoes,
            Alerts = alerts,
        };
    }

    public async Task<DashboardAnalyticsDto> GetAnalyticsAsync()
    {
        var users = await _userService.GetAllAsync();
        var transactions = await _transactionService.GetAllAsync();

        var programas = users
            .Where(u => u.Profile != null)
            .GroupBy(u => u.Profile!.PreferredLoyaltyProgram.ToString())
            .Select(g => new ProgramaAnalyticsDto
            {
                Name = g.Key,
                Miles = g.Sum(u => u.Profile!.CurrentMilesBalance),
                Fill = ProgramVisuals.Hex(g.Key),
            })
            .ToList();

        var evolucao = transactions
            .GroupBy(t => t.Date.ToString("MMM"))
            .Select(g => new MonthMilesDto
            {
                Month = g.Key,
                Miles = g.Sum(t => t.MilesEarned),
            })
            .ToList();

        return new DashboardAnalyticsDto
        {
            Programas = programas,
            Distribuicao = programas,
            Evolucao = evolucao,
            Vencimento = new List<MonthMilesDto>
            {
                new() { Month = "Mai", Miles = 8000 },
                new() { Month = "Jun", Miles = 3000 },
                new() { Month = "Jul", Miles = 12000 },
                new() { Month = "Ago", Miles = 6000 },
                new() { Month = "Set", Miles = 4000 },
            },
            Crescimento = "+4.000",
            AVencer = "5.000",
            TotalVencimento = "38.000",
        };
    }
}
