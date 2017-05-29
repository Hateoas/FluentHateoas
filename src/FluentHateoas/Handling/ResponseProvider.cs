using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using FluentHateoas.Registration;

namespace FluentHateoas.Handling
{
    public class ResponseProvider : IResponseProvider
    {
        private readonly IConfigurationProvider _configurationProvider;

        public ResponseProvider(IConfigurationProvider configurationProvider)
        {
            _configurationProvider = configurationProvider;
        }

        public IEnumerable<IHateoasLink> CreateLinks(HttpResponseMessage response)
        {
            ObjectContent content;
            if (!response.TryGetContent(out content) || content == null)
            {
                return new List<IHateoasLink>();
            }

            return _configurationProvider.GetLinksFor(content.Value.GetType(), content.Value);
        }
    }
}