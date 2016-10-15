using System.Collections.Generic;

namespace FluentHateoas.Handling
{
    public interface IConfigurationProvider
    {
        IEnumerable<IHateoasLink> GetLinksFor<TModel>(TModel data);
        IEnumerable<IHateoasLink> GetLinksFor(System.Type modelType, object data);
    }
}