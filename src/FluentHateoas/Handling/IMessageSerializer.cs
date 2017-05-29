using System.Collections.Generic;
using System.Net.Http;
using System.Web;

namespace FluentHateoas.Handling
{
    public interface IMessageSerializer
    {
        bool HandlesContentType(string contentType);

        HttpRequestMessage OnRequest(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken);
        HttpResponseMessage OnResponse<TModel>(HttpRequestMessage request, HttpResponseMessage response, TModel model, IEnumerable<IHateoasLink> links);
    }
}