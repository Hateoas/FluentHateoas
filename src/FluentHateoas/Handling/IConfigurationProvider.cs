using System;
using System.Collections.Generic;

namespace FluentHateoas.Handling
{
    public interface IConfigurationProvider
    {
        IEnumerable<IHateoasLink> GetLinksFor(Type type, object data);

        IEnumerable<IHateoasLink> GetLinksFor<TModel>(TModel data);
        IEnumerable<IHateoasLink> GetLinksFor<TModel>(IEnumerable<TModel> data);
    }
}