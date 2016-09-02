namespace FluentHateoas.Handling
{
    public interface ILinkFactory
    {
        System.Collections.Generic.IEnumerable<IHateoasLink> CreateLinks(System.Collections.Generic.List<Interfaces.IHateoasRegistration> registrations, object data);
    }
}