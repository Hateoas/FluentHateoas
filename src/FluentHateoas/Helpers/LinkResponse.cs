namespace FluentHateoas.Helpers
{
    public class LinkResponse
    {
        public string Href { get; set; }
        public string Template { get; set; }
        public string Rel { get; set; }
        public string Method { get; set; }
        public string Command { get; set; }
    }
}