using System;
using System.Text.Json.Serialization;

namespace MilhasAPI.Models;

public class RewardTransaction
{
    public int Id { get; private set; }
    public int UserId { get; set; }

    [JsonIgnore]
    public User? User { get; set; }

    public int CreditCardId { get; set; }

    [JsonIgnore]
    public CreditCard? CreditCard { get; set; }

    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public int MilesEarned { get; set; }
    public LoyaltyProgram? Program { get; set; }
}
