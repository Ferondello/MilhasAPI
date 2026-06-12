namespace MilhasAPI.Utils;

public static class CardNumberMasker
{
    public static string Mask(string number)
    {
        var digits = number.Replace(" ", "");
        if (digits.Length < 4) return "•••• •••• •••• ????";
        return $"•••• •••• •••• {digits[^4..]}";
    }
}
