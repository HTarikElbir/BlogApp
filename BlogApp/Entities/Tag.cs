namespace BlogApp.Entities
{
    public enum TagColour 
    {
        primary, secondary, success, danger, warning, info, light, dark
    }
    public class Tag
    {
        public int TagId { get; set; }
        public string? Text { get; set; }
        public string? Url { get; set; }
        public TagColour? Colour { get; set; }
        public List<Post> Posts { get; set; } = new List<Post>();
    }
}
