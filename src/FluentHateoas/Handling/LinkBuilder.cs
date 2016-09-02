namespace FluentHateoas.Handling
{
    public class LinkBuilder
    {
        public string Relation { get; set; }
        public object Id { get; set; }
        public System.Type Controller { get; set; }
        public bool Success { get; set; }
        public System.Net.Http.HttpMethod Method { get; set; }
        public bool IsTemplate { get; set; }
        public bool IsFixed { get; set; }
        public string FixedRoute { get; set; }
        public string Command { get; set; }
    }
}