using System.Collections.Generic;
using System.Net.Http;

namespace FluentHateoas.Handling
{
    public interface IResponseProvider
    {
        IEnumerable<IHateoasLink> CreateLinks(HttpResponseMessage response);
    }
}