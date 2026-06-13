using MilhasAPI.Models;

namespace MilhasAPI.Services.Interfaces;

/// <summary>
/// Agrega dados de usuários e transações em respostas prontas para as telas
/// Home e de gráficos. Concentra a regra de agregação fora do controller.
/// </summary>
public interface IDashboardService
{
    Task<DashboardResumoDto> GetResumoAsync();
    Task<DashboardAnalyticsDto> GetAnalyticsAsync();
}
