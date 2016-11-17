namespace FluentHateoas.Handling
{
    public interface ILinkBuilderFactory
    {
        ILinkBuilder GetLinkBuilder(object data);
    }
}