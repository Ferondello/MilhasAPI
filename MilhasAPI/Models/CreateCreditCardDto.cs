namespace MilhasAPI.Models;

public class CreateCreditCardDto
{
    public string CardNumber { get; set; } = null!;
    public string Brand { get; set; } = null!;
    public LoyaltyProgram? Program { get; set; }
    public int UserId { get; set; }
}
