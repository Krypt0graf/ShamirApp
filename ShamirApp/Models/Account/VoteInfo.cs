using System.Text.Json.Serialization;

namespace ShamirApp.Models.Account
{
    public class VoteInfo
    {
        [JsonPropertyName("id")] public int IdQuestion { get; set; }
        [JsonPropertyName("points")] public List<Point>? Points { get; set; }
    }

    public class Point
    {
        [JsonPropertyName("x")] public int X { get; set; }
        [JsonPropertyName("y")] public int Y { get; set; }
    }
}
