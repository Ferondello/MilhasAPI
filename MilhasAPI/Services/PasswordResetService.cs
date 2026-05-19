using MilhasAPI.Services.Interfaces;

namespace MilhasAPI.Services;

/// <summary>
/// Armazena códigos de reset em memória (singleton).
/// Cada código expira em 15 minutos e é removido após uso.
/// </summary>
public class PasswordResetService : IPasswordResetService
{
    private record Entry(string Code, DateTime ExpiresAt);

    private readonly Dictionary<string, Entry> _store = new(StringComparer.OrdinalIgnoreCase);

    public string GenerateCode(string email)
    {
        var code = Random.Shared.Next(100_000, 999_999).ToString();
        _store[email] = new Entry(code, DateTime.UtcNow.AddMinutes(15));
        return code;
    }

    public bool ValidateCode(string email, string code)
    {
        if (!_store.TryGetValue(email, out var entry))
            return false;

        if (DateTime.UtcNow > entry.ExpiresAt)
        {
            _store.Remove(email);
            return false;
        }

        return entry.Code == code;
    }

    public void RemoveCode(string email) => _store.Remove(email);
}
