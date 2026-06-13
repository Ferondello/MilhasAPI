using MilhasAPI.Scrapers.Models;

namespace MilhasAPI.Scrapers.Interfaces;

/// <summary>
/// Gera uma cotação estimada para um programa quando a coleta real não trouxe
/// dado para ele no ciclo atual. Abstraído para permitir trocar a estratégia
/// (ranges fixos, configuração, modelo estatístico) sem alterar o orquestrador.
/// </summary>
public interface IMilesQuoteEstimator
{
    MilesQuote GenerateFor(string program);
}
