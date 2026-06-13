using MilhasAPI.Services.Interfaces;

namespace MilhasAPI.Services;

public class EmailTemplateRenderer : IEmailTemplateRenderer
{
    public string RenderPasswordReset(string name, string code) => $"""
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
