namespace FluentHateoas.Handling
{
    public interface ILinkFactory
    {
        System.Collections.Generic.IEnumerable<IHateoasLink> CreateLinks<TModel>(System.Collections.Generic.List<Interfaces.IHateoasRegistration<TModel>> registrations, TModel data);
    }
}