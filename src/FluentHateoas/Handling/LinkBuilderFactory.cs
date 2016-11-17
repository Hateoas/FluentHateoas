namespace FluentHateoas.Handling
{
    public class LinkBuilderFactory : ILinkBuilderFactory
    {
        public ILinkBuilder GetLinkBuilder(object data)
        {
            return new LinkBuilder(data);
        }
    }
}