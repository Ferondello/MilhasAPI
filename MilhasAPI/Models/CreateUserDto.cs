using System.Collections.Generic;

namespace MilhasAPI.Models;

public class CreateUserDto
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public ICollection<CreateCreditCardDto>? CreditCards { get; set; }
}
