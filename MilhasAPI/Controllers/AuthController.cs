using Microsoft.AspNetCore.Mvc;
using MilhasAPI.Models;
using MilhasAPI.Services.Interfaces;

namespace MilhasAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService           _authService;
    private readonly ITokenService          _tokenService;
    private readonly IEmailService          _emailService;
    private readonly IEmailTemplateRenderer _templateRenderer;
    private readonly IPasswordResetService  _resetService;

    public AuthController(
        IAuthService           authService,
        ITokenService          tokenService,
        IEmailService          emailService,
        IEmailTemplateRenderer templateRenderer,
        IPasswordResetService  resetService)
    {
        _authService      = authService;
        _tokenService     = tokenService;
        _emailService     = emailService;
        _templateRenderer = templateRenderer;
        _resetService     = resetService;
    }

    /// <summary>POST /api/auth/register</summary>
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var (user, error) = await _authService.RegisterAsync(dto);

        if (error != null)
            return Conflict(new { message = error });

        var token = _tokenService.GenerateToken(user!);

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

        var user = await _authService.ValidateCredentialsAsync(dto.Email, dto.Password);

        if (user == null)
            return Unauthorized(new { message = "E-mail ou senha inválidos." });

        var token = _tokenService.GenerateToken(user);

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
        var user = await _authService.GetByEmailAsync(dto.Email);

        if (user != null)
        {
            var code = _resetService.GenerateCode(dto.Email);
            await _emailService.SendAsync(
                dto.Email,
                user.Name,
                "Redefinição de senha — MilhasGerais",
                _templateRenderer.RenderPasswordReset(user.Name, code)
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

        var success = await _authService.ResetPasswordAsync(dto.Email, dto.NewPassword);
        if (!success)
            return NotFound(new { message = "Usuário não encontrado." });

        _resetService.RemoveCode(dto.Email);
        return Ok(new { message = "Senha redefinida com sucesso." });
    }
}
