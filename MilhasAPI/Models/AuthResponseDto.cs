namespace MilhasAPI.Models;

public class AuthResponseDto
{
    public string Token { get; set; } = null!;
    public UserResponseDto User { get; set; } = null!;
}

public class UserResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
}
