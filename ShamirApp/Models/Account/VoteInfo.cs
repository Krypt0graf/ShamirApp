using System.Text.Json.Serialization;

namespace ShamirApp.Models.Account
{
    public class VoteInfo
    {
        [JsonPropertyName("id")] public int Id { get; set; }
        [JsonPropertyName("points")] public List<Point>? Points { get; set; } = new List<Point>();
    }

    public class Point
    {
        [JsonPropertyName("x")] public int X { get; set; }
        [JsonPropertyName("y")] public int Y { get; set; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
