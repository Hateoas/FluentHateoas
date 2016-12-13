using System.Linq;
using System.Net.Http;
using FluentHateoas.Helpers;

namespace FluentHateoas.Handling
{
    public static class JsonApiResponseHelper
    {
        public static HttpResponseMessage Ok(HttpRequestMessage request, object model, System.Collections.Generic.IEnumerable<IHateoasLink> links)
        {
            return CreateResponse(request, CreateHateoasResponse(model, links), System.Net.HttpStatusCode.OK);
        }

        private static JsonApiResponse CreateHateoasResponse(object model, System.Collections.Generic.IEnumerable<IHateoasLink> links)
        {
            return new JsonApiResponse
            {
                Type = model.GetType().Name
            };

            //Data = model,
            //Links = links.ToLinkList(),
            //Commands = links.Where(p => p.Command != null).Select(p => p.Command)
        }

        private static HttpResponseMessage CreateResponse(HttpRequestMessage request, JsonApiResponse response, System.Net.HttpStatusCode statusCode)
        {
            return request.CreateResponse(statusCode, response);
        }
    }
}