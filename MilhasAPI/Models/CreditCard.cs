namespace MilhasAPI.Models;

public class CreditCard
{
    public int Id { get; set; }
    public string CardNumber { get; set; } = null!;
    public string Brand { get; set; } = null!;
    public int UserId { get; set; }
    public User? User { get; set; }
}
