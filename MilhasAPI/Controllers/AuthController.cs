using Microsoft.AspNetCore.Mvc;
using MilhasAPI.Models;
using MilhasAPI.Services.Interfaces;

namespace MilhasAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService          _userService;
    private readonly IEmailService         _emailService;
    private readonly IPasswordResetService _resetService;

    public AuthController(
        IUserService          userService,
        IEmailService         emailService,
        IPasswordResetService resetService)
    {
        _userService  = userService;
        _emailService = emailService;
        _resetService = resetService;
    }

    /// <summary>POST /api/auth/register</summary>
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
            User  = new UserResponseDto { Id = user!.Id, Name = user.Name, Email = user.Email }
        });
    }

    /// <summary>POST /api/auth/login</summary>
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
            User  = new UserResponseDto { Id = user.Id, Name = user.Name, Email = user.Email }
        });
    }

    /// <summary>
    /// Gera um código de 6 dígitos e envia por e-mail.
    /// Sempre retorna 200 para não revelar se o e-mail existe.
    /// POST /api/auth/forgot-password
    /// </summary>
    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
    {
        var user = await _userService.GetByEmailAsync(dto.Email);

        if (user != null)
        {
            var code = _resetService.GenerateCode(dto.Email);
            await _emailService.SendAsync(
                dto.Email,
                user.Name,
                "Redefinição de senha — MilhasGerais",
                BuildResetEmail(user.Name, code)
            );
        }

        return Ok(new { message = "Se esse e-mail estiver cadastrado, você receberá o código em breve." });
    }

    /// <summary>
    /// Valida o código e atualiza a senha.
    /// POST /api/auth/reset-password
    /// </summary>
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
    {
        if (!_resetService.ValidateCode(dto.Email, dto.Code))
            return BadRequest(new { message = "Código inválido ou expirado." });

        var success = await _userService.ResetPasswordAsync(dto.Email, dto.NewPassword);
        if (!success)
            return NotFound(new { message = "Usuário não encontrado." });

        _resetService.RemoveCode(dto.Email);
        return Ok(new { message = "Senha redefinida com sucesso." });
    }

    private static string GenerateToken(User user)
        => $"{user.Id}_{user.Email}_{Guid.NewGuid():N}";

    private static string BuildResetEmail(string name, string code) => $"""
        <!DOCTYPE html>
        <html lang="pt-BR">
        <body style="font-family:sans-serif;background:#f7f5f2;padding:32px;">
          <div style="max-width:480px;margin:auto;background:#fff;border-radius:24px;padding:40px;box-shadow:0 4px 24px rgba(0,0,0,.08);">
            <h1 style="color:#1b3a5c;font-size:24px;margin-bottom:8px;">✈️ MilhasGerais</h1>
            <p style="color:#2c2c2c;">Olá, <strong>{name}</strong>!</p>
            <p style="color:#2c2c2c;">Recebemos uma solicitação para redefinir sua senha. Use o código abaixo:</p>
            <div style="text-align:center;margin:32px 0;">
              <span style="font-size:40px;font-weight:700;letter-spacing:12px;color:#1b3a5c;">{code}</span>
            </div>
            <p style="color:#7a7a7a;font-size:14px;">Este código é válido por <strong>15 minutos</strong>.</p>
            <p style="color:#7a7a7a;font-size:14px;">Se você não solicitou a redefinição, ignore este e-mail.</p>
          </div>
        </body>
        </html>
        """;
}
