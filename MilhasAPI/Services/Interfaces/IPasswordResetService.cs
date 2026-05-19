namespace MilhasAPI.Services.Interfaces;

public interface IPasswordResetService
{
    string  GenerateCode(string email);
    bool    ValidateCode(string email, string code);
    void    RemoveCode(string email);
}
