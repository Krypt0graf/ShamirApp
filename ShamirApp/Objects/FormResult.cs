namespace ShamirApp.Objects
{
    public class FormResult
    {
        public int CountVotes { get; set; }
        public List<Info> Results { get; set; } = new List<Info>();
    }
    public class Info
    {
        public int IdQuestion { get; set; }
        public string Text { get; set; } = string.Empty;
        public int Value { get; set; }
    }
}
