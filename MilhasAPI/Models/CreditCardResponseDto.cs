namespace MilhasAPI.Models;

/// <summary>
/// Representação de saída de um cartão. O número já vai mascarado, sem expor
/// a entidade de persistência nem arriscar persistir o valor mascarado.
/// </summary>
public class CreditCardResponseDto
{
    public int Id { get; set; }
    public string CardNumber { get; set; } = null!;
    public string Brand { get; set; } = null!;
    public LoyaltyProgram? Program { get; set; }
    public int UserId { get; set; }
}
