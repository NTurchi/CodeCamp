namespace MyCodeCamp.Models
{
    public class LinkModel
    {
        public string Href { get; set; }

        public string Rel { get; set; }

        public string Verb { get; set; } = "GET";
    }
}