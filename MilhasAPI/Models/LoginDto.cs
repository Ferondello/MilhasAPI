using System.ComponentModel.DataAnnotations;

namespace MilhasAPI.Models;

public class LoginDto
{
    [Required(ErrorMessage = "E-mail é obrigatório")]
    [EmailAddress(ErrorMessage = "E-mail inválido")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Senha é obrigatória")]
    public string Password { get; set; } = null!;
}
