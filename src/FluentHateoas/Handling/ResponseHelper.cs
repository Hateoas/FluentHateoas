using System.Linq;
using System.Net.Http;
using FluentHateoas.Helpers;

namespace FluentHateoas.Handling
{
    public static class ResponseHelper
    {
        public static HttpResponseMessage Ok(HttpRequestMessage request, object model, System.Collections.Generic.IEnumerable<IHateoasLink> links, System.Collections.Generic.IEnumerable<IHateoasCommand> commands)
        {
            return CreateResponse(request, CreateHateoasResponse(model, links, commands), System.Net.HttpStatusCode.OK);
        }

        private static HateoasResponse CreateHateoasResponse(object model, System.Collections.Generic.IEnumerable<IHateoasLink> links, System.Collections.Generic.IEnumerable<IHateoasCommand> commands)
        {
            return new HateoasResponse
            {
                Data = model,
                Links = links.ToLinkList(),
                Commands = links.Where(p => p.Command != null).Select(p => p.Command)
            };
        }

        private static HttpResponseMessage CreateResponse(HttpRequestMessage request, HateoasResponse response, System.Net.HttpStatusCode statusCode)
        {
            return request.CreateResponse(statusCode, response);
        }
    }
}