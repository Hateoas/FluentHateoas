namespace FluentHateoas.Handling
{
    public interface IHateoasLink
    {
        string Rel { get; set; }
        string Href { get; set; }
        string Method { get; set; }
        string Template { get; set; }
        string Command { get; set; }
    }

    public class HateoasLink : IHateoasLink
    {
        public string Rel { get; set; }
        public string Href { get; set; }
        public string Method { get; set; }
        public string Template { get; set; }
        public string Command { get; set; }
    }
}