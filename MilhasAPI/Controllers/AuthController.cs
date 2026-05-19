using Microsoft.AspNetCore.Mvc;
using MilhasAPI.Models;
using MilhasAPI.Services.Interfaces;

namespace MilhasAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;

    public AuthController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Registra um novo usuário.
    /// POST /api/auth/register
    /// </summary>
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var (user, error) = await _userService.RegisterAsync(dto);

        if (error != null)
            return Conflict(new { message = error });

        var token = GenerateToken(user!);

        return CreatedAtAction(nameof(Register), new AuthResponseDto
        {
            Token = token,
            User = new UserResponseDto
            {
                Id   = user!.Id,
                Name = user.Name,
                Email = user.Email
            }
        });
    }

    /// <summary>
    /// Autentica um usuário existente.
    /// POST /api/auth/login
    /// </summary>
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _userService.ValidateCredentialsAsync(dto.Email, dto.Password);

        if (user == null)
            return Unauthorized(new { message = "E-mail ou senha inválidos." });

        var token = GenerateToken(user);

        return Ok(new AuthResponseDto
        {
            Token = token,
            User = new UserResponseDto
            {
                Id    = user.Id,
                Name  = user.Name,
                Email = user.Email
            }
        });
    }

    // Gera um token simples baseado em GUID.
    // Para produção, substitua por JWT (adicione Microsoft.AspNetCore.Authentication.JwtBearer).
    private static string GenerateToken(User user)
        => $"{user.Id}_{user.Email}_{Guid.NewGuid():N}";
}
