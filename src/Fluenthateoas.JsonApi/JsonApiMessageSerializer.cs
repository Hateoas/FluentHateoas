using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using Fluenthateoas.JsonApi.Handlers;
using FluentHateoas.Handling;
using FluentHateoas.Registration;

namespace Fluenthateoas.JsonApi
{
    public class JsonApiMessageSerializer : MessageSerializerBase
    {
        public override bool HandlesContentType(string contentType)
        {
            return contentType == "application/vnd.api+json";
        }

        public override HttpRequestMessage OnRequest(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return request;
        }

        public override HttpResponseMessage OnResponse<TModel>(HttpRequestMessage request, HttpResponseMessage response, TModel model,
            IEnumerable<IHateoasLink> links)
        {
            return AddToResponse(response, JsonApiResponseHelper.Ok(request, ((ObjectContent)response.Content).Value, links, NullValueHandling.Ignore));
        }
    }
}