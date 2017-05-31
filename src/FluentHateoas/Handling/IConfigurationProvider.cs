using System;
using System.Collections.Generic;
using FluentHateoas.Registration;

namespace FluentHateoas.Handling
{
    public interface IConfigurationProvider
    {
        IEnumerable<IHateoasLink> GetLinksFor(Type type, object data);

        IEnumerable<IHateoasLink> GetLinksFor<TModel>(TModel data);
        IEnumerable<IHateoasLink> GetLinksFor<TModel>(IEnumerable<TModel> data);
        NullValueHandling GetNullValueHandling();
    }
}