using Microsoft.AspNetCore.Mvc;
using MilhasAPI.Services.Interfaces;

namespace MilhasAPI.Controllers;

/// <summary>
/// Agrega dados de múltiplos serviços em respostas prontas para o frontend.
/// Evita que o frontend faça múltiplas chamadas para montar a tela home.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IRewardTransactionService _transactionService;

    public DashboardController(IUserService userService, IRewardTransactionService transactionService)
    {
        _userService = userService;
        _transactionService = transactionService;
    }

    /// <summary>
    /// Retorna resumo consolidado para a tela Home.
    /// GET /api/dashboard/resumo
    /// </summary>
    [HttpGet("resumo")]
    public async Task<IActionResult> GetResumo()
    {
        var users = await _userService.GetAllAsync();
        var transactions = await _transactionService.GetAllAsync();

        // Calcula saldo total de milhas somando todos os perfis
        var totalMiles = users
            .Where(u => u.Profile != null)
            .Sum(u => u.Profile!.CurrentMilesBalance);

        // Programas consolidados (um por LoyaltyProgram distinto nos perfis)
        var programas = users
            .Where(u => u.Profile != null)
            .GroupBy(u => u.Profile!.PreferredLoyaltyProgram.ToString())
            .Select((g, i) => new
            {
                id = (i + 1).ToString(),
                name = g.Key,
                miles = g.Sum(u => u.Profile!.CurrentMilesBalance),
                color = ProgramColor(g.Key),
                logo = ProgramLogo(g.Key),
            })
            .ToList();

        // Últimas 10 transações formatadas para o front
        var transacoes = transactions
            .OrderByDescending(t => t.Date)
            .Take(10)
            .Select(t => new
            {
                id = t.Id.ToString(),
                type = "credit",
                program = t.CreditCard?.Brand ?? "Cartão",
                amount = t.MilesEarned,
                date = t.Date.ToString("dd/MM/yyyy"),
                description = $"Compra R$ {t.Amount:N2}",
            })
            .ToList();

        // Alertas estáticos (aqui você pode adicionar lógica real de expiração depois)
        var alerts = new[]
        {
            new { id = "1", text = "Verifique as cotações — há promoções ativas!", type = "info" },
        };

        return Ok(new
        {
            totalMiles,
            crescimento = 4000,
            aVencer = 5000,
            programas,
            transacoes,
            alerts,
        });
    }

    /// <summary>
    /// Retorna dados analíticos para a tela de gráficos.
    /// GET /api/dashboard/analytics
    /// </summary>
    [HttpGet("analytics")]
    public async Task<IActionResult> GetAnalytics()
    {
        var users = await _userService.GetAllAsync();
        var transactions = await _transactionService.GetAllAsync();

        var programas = users
            .Where(u => u.Profile != null)
            .GroupBy(u => u.Profile!.PreferredLoyaltyProgram.ToString())
            .Select((g, i) => new
            {
                name = g.Key,
                miles = g.Sum(u => u.Profile!.CurrentMilesBalance),
                fill = ProgramHex(g.Key),
            })
            .ToList();

        // Evolução mensal baseada nas transações reais
        var evolucao = transactions
            .GroupBy(t => t.Date.ToString("MMM"))
            .Select(g => new
            {
                month = g.Key,
                miles = g.Sum(t => t.MilesEarned),
            })
            .ToList();

        return Ok(new
        {
            programas,
            distribuicao = programas,
            evolucao,
            vencimento = new[]
            {
                new { month = "Mai", miles = 8000 },
                new { month = "Jun", miles = 3000 },
                new { month = "Jul", miles = 12000 },
                new { month = "Ago", miles = 6000 },
                new { month = "Set", miles = 4000 },
            },
            crescimento = "+4.000",
            aVencer = "5.000",
            totalVencimento = "38.000",
        });
    }

    private static string ProgramColor(string program) => program switch
    {
        "Smiles"    => "bg-[#054A91]",
        "Latam"     => "bg-[#6EA4BF]",
        "Azul"      => "bg-blue-500",
        "Livelo"    => "bg-[#748944]",
        "Esfera"    => "bg-purple-500",
        "Multiplus" => "bg-orange-500",
        _           => "bg-gray-400",
    };

    private static string ProgramLogo(string program) => program switch
    {
        "Smiles"    => "✈️",
        "Latam"     => "🛫",
        "Azul"      => "🛩️",
        "Livelo"    => "🎯",
        "Esfera"    => "💎",
        "Multiplus" => "⭐",
        _           => "🏆",
    };

    private static string ProgramHex(string program) => program switch
    {
        "Smiles"    => "#054A91",
        "Latam"     => "#6EA4BF",
        "Azul"      => "#3B82F6",
        "Livelo"    => "#748944",
        "Esfera"    => "#8B5CF6",
        "Multiplus" => "#F97316",
        _           => "#6B7280",
    };
}
