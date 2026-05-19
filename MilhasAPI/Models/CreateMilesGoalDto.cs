using System.ComponentModel.DataAnnotations;

namespace MilhasAPI.Models;

public class CreateMilesGoalDto
{
    [Required(ErrorMessage = "Nome da meta é obrigatório.")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "Nome deve ter entre 1 e 100 caracteres.")]
    public string Name { get; set; } = null!;

    [Range(1, int.MaxValue, ErrorMessage = "Meta de milhas deve ser maior que zero.")]
    public int TargetMiles { get; set; }

    [Range(1, int.MaxValue)]
    public int UserId { get; set; }
}
