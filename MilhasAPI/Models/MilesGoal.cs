using System.Text.Json.Serialization;

namespace MilhasAPI.Models;

public class MilesGoal
{
    public int Id { get; private set; }
    public int UserId { get; set; }
    public string Name { get; set; } = null!;
    public int TargetMiles { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [JsonIgnore]
    public User? User { get; set; }
}
