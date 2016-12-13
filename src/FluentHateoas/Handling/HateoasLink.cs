namespace FluentHateoas.Handling
{
    public interface IHateoasLink
    {
        string Relation { get; set; }
        bool IsMember { get; set; }
        string LinkPath { get; set; }
        string Method { get; set; }
        string Template { get; set; }
        IHateoasCommand Command { get; set; }
    }

    public class HateoasLink : IHateoasLink
    {
        public string Relation { get; set; }
        public string LinkPath { get; set; }
        public string Method { get; set; }
        public string Template { get; set; }
        public IHateoasCommand Command { get; set; }
        public bool IsMember { get; set; }
    }
}