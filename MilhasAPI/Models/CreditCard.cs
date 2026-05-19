using System.Text.Json.Serialization;

namespace MilhasAPI.Models;

public class CreditCard
{
    public int Id { get; private set; }
    public string CardNumber { get; set; } = null!;
    public string Brand { get; set; } = null!;
    public LoyaltyProgram? Program { get; set; }
    public int UserId { get; set; }

    [JsonIgnore]
    public User? User { get; set; }
}
