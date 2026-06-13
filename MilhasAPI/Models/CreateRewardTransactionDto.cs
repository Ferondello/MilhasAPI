using System.ComponentModel.DataAnnotations;

namespace MilhasAPI.Models;

/// <summary>
/// Entrada para registrar uma transação de recompensa. Evita receber a entidade
/// de persistência diretamente no controller (over-posting).
/// </summary>
public class CreateRewardTransactionDto
{
    [Required]
    public int UserId { get; set; }

    [Required]
    public int CreditCardId { get; set; }

    public DateTime Date { get; set; }

    public decimal Amount { get; set; }

    public int MilesEarned { get; set; }

    public LoyaltyProgram? Program { get; set; }
}
