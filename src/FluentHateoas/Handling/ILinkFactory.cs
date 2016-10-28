using System.Collections.Generic;

namespace FluentHateoas.Handling
{
    public interface ILinkFactory
    {
        IEnumerable<IHateoasLink> CreateLinks<TModel>(IEnumerable<Interfaces.IHateoasRegistration<TModel>> registrations, object data);
    }
}