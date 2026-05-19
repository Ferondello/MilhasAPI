using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MilhasAPI.Models;

public class User
{
    public int Id { get; private set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;

    [JsonIgnore]
    public string? PasswordHash { get; set; }

    public ICollection<CreditCard> CreditCards { get; set; } = new List<CreditCard>();
    public UserProfile? Profile { get; set; }
}
