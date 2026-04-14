using System.Collections.Generic;

namespace MilhasAPI.Models;

public class CreateUserDto
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    // Optional: client may provide credit cards when creating a user, but it's not required
    public ICollection<CreditCard>? CreditCards { get; set; }
}
