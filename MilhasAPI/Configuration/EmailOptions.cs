namespace MilhasAPI.Configuration;

/// <summary>
/// Configurações de envio de e-mail, vinculadas da seção "Email" do appsettings
/// via Options pattern. Substitui o acesso direto a IConfiguration com chaves mágicas.
/// </summary>
public class EmailOptions
{
    public const string SectionName = "Email";

    public string FromName { get; set; } = "MilhasGerais";
    public string FromAddress { get; set; } = "";
    public string SmtpHost { get; set; } = "";
    public int SmtpPort { get; set; } = 587;
    public bool UseSsl { get; set; } = false;
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
}
