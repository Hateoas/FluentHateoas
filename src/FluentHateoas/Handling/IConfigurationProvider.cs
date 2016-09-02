namespace FluentHateoas.Handling
{
    public interface IConfigurationProvider
    {
        System.Collections.Generic.IEnumerable<IHateoasLink> GetLinksFor<TModel>(object data);
        System.Collections.Generic.IEnumerable<IHateoasLink> GetLinksFor(System.Type modelType, object data);
    }
}