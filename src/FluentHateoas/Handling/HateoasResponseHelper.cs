using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using FluentHateoas.Helpers;

namespace FluentHateoas.Handling
{
    public static class HateoasResponseHelper
    {
        public static HateoasResponse CreateResponseContent(HttpRequestMessage request, object model, IEnumerable<IHateoasLink> links)
        {
            return CreateHateoasResponse(model, links);
        }

        private static HateoasResponse CreateHateoasResponse(object model, IEnumerable<IHateoasLink> links)
        {
            return new HateoasResponse
            {
                Data = model,
                Links = links.ToLinkList(),
                Commands = links.Where(p => p.Command != null).Select(p => p.Command)
            };
        }
    }
}