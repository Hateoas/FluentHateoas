using System.Collections.Generic;

namespace FluentHateoas.Handling
{
    public interface IConfigurationProvider
    {
        IEnumerable<IHateoasLink> GetLinksFor<TModel>(object data);
        IEnumerable<IHateoasLink> GetLinksFor(System.Type modelType, object data);
    }
}