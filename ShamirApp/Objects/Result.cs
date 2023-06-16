using ShamirApp.Models.Account;

namespace ShamirApp.Objects
{
    public class Result
    {
        public int IdForm { get; set; }
        public int IdUser { get; set; }
        public int IdQuestion { get; set; }
        public List<Point> Points { get; set; } = new List<Point>();
    }
}
