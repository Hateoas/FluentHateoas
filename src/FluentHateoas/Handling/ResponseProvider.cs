using System.Collections.Generic;
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

            // Yes walk on, nothing to see here
            var links = _configurationProvider.GetLinksFor(content.Value.GetType(), content.Value);

            //var links = _configurationProvider
            //    .GetType()
            //    .GetMethod(nameof(_configurationProvider.GetLinksFor), new [] { content.Value.GetType() })
            //    // .MakeGenericMethod(content.Value.GetType())
            //    .Invoke(_configurationProvider, new [] { content.Value }) as IEnumerable<IHateoasLink>;

            var commands = new List<IHateoasCommand>();
            return ResponseHelper.Ok(request, ((ObjectContent)response.Content).Value, links, commands);
        }
    }
}