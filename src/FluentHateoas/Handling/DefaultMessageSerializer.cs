using System.Collections.Generic;
using System.Net.Http;
using System.Threading;

namespace FluentHateoas.Handling
{
    public abstract class MessageSerializeBase : IMessageSerializer
    {
        public abstract bool HandlesContentType(string contentType);
        public abstract HttpRequestMessage OnRequest(HttpRequestMessage request, CancellationToken cancellationToken);
        public abstract HttpResponseMessage OnResponse<TModel>(HttpRequestMessage request, HttpResponseMessage response, TModel model, IEnumerable<IHateoasLink> links);

        protected static HttpResponseMessage AddToResponse<TModel>(HttpResponseMessage response, TModel model)
        {
            response.Content = new ObjectContent<TModel>(model, ((ObjectContent) response.Content).Formatter);
            return response;
        }
    }

    public class DefaultMessageSerializer : MessageSerializeBase
    {
        public override bool HandlesContentType(string contentType)
        {
            return true;
        }

        public override HttpRequestMessage OnRequest(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return request;
        }

        public override HttpResponseMessage OnResponse<TModel>(HttpRequestMessage request, HttpResponseMessage response, TModel model, IEnumerable<IHateoasLink> links)
        {
            return AddToResponse(response, HateoasResponseHelper.CreateResponseContent(request, ((ObjectContent)response.Content).Value, links));
        }
    }
}