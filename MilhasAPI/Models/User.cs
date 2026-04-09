namespace MilhasAPI.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public ICollection<CreditCard>? CreditCards { get; set; }
}
