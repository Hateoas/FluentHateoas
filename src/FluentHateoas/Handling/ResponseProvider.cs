using System.Linq;
using System.Net.Http;

namespace FluentHateoas.Handling
{
    public class ResponseProvider : IResponseProvider
    {
        private readonly IConfigurationProvider _configurationProvider;

        public ResponseProvider(IConfigurationProvider configurationProvider)
        {
            _configurationProvider = configurationProvider;
        }

        public HttpResponseMessage Create(HttpRequestMessage request, HttpResponseMessage response)
        {
            ObjectContent content;
            if (!response.TryGetContent(out content) || content == null)
            {
                return response;
            }

            var links = _configurationProvider.GetLinksFor(content.Value);//content.ObjectType, 
            var commands = new System.Collections.Generic.List<IHateoasCommand>(); // TODO
            return ResponseHelper.Ok(request, ((ObjectContent)(response.Content)).Value, links, commands);
        }
    }
}