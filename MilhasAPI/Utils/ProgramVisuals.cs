namespace MilhasAPI.Utils;

/// <summary>
/// Mapeia o nome de um programa para seus atributos visuais (cor Tailwind, emoji,
/// cor hex). Tabela de dados única — adicionar um programa é incluir uma entrada,
/// sem espalhar switches pelos controllers.
/// </summary>
public static class ProgramVisuals
{
    private record Visual(string Color, string Logo, string Hex);

    private static readonly Dictionary<string, Visual> Map = new()
    {
        ["Smiles"]    = new("bg-[#054A91]", "✈️", "#054A91"),
        ["Latam"]     = new("bg-[#6EA4BF]", "🛫", "#6EA4BF"),
        ["Azul"]      = new("bg-blue-500", "🛩️", "#3B82F6"),
        ["Livelo"]    = new("bg-[#748944]", "🎯", "#748944"),
        ["Esfera"]    = new("bg-purple-500", "💎", "#8B5CF6"),
        ["Multiplus"] = new("bg-orange-500", "⭐", "#F97316"),
    };

    private static readonly Visual Fallback = new("bg-gray-400", "🏆", "#6B7280");

    public static string Color(string program) => Resolve(program).Color;
    public static string Logo(string program) => Resolve(program).Logo;
    public static string Hex(string program) => Resolve(program).Hex;

    private static Visual Resolve(string program) =>
        Map.TryGetValue(program, out var v) ? v : Fallback;
}
