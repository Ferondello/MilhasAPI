using System;

namespace MilhasAPI.Models;

public class RewardTransaction
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
    public int CreditCardId { get; set; }
    public CreditCard? CreditCard { get; set; }
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public int MilesEarned { get; set; }
}
