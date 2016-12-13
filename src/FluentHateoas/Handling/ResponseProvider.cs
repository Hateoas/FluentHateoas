using System;
using System.Collections.Generic;
using System.Net.Http;
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

        public HttpResponseMessage Create(HttpRequestMessage request, HttpResponseMessage response)
        {
            ObjectContent content;
            if (!response.TryGetContent(out content) || content == null)
            {
                return response;
            }

            var links = _configurationProvider.GetLinksFor(content.Value.GetType(), content.Value);

            switch (_configurationProvider.GetResponseStyle())
            {
                case ResponseStyle.JsonApi:
                    return JsonApiResponseHelper.Ok(request, ((ObjectContent)response.Content).Value, links);
                default:
                    return HateoasResponseHelper.Ok(request, ((ObjectContent)response.Content).Value, links);
            }
        }
    }
}