namespace MilhasAPI.Services.Interfaces;

/// <summary>
/// Monta os corpos HTML dos e-mails transacionais. Mantém o markup fora dos
/// controllers (apresentação não é responsabilidade do controller).
/// </summary>
public interface IEmailTemplateRenderer
{
    string RenderPasswordReset(string name, string code);
}
