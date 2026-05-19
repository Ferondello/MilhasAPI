namespace MilhasAPI.Models;

public class UpdateCreditCardDto
{
    public string? CardNumber { get; set; }
    public string? Brand { get; set; }
    public LoyaltyProgram? Program { get; set; }
}
