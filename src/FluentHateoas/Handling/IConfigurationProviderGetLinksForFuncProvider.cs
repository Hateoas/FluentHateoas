using System;
using System.Collections.Generic;

namespace FluentHateoas.Handling
{
    public interface IConfigurationProviderGetLinksForFuncProvider
    {
        Func<IConfigurationProvider, object, IEnumerable<IHateoasLink>> GetLinksForFunc(IConfigurationProvider configurationProvider, Type singleContentType, Type contentTypeToUse);
        Func<IConfigurationProvider, object, IEnumerable<IHateoasLink>> GetLinksForFunc(IConfigurationProvider configurationProvider, Type contentType, object content);
    }
}