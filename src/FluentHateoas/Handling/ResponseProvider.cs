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

        public HttpResponseMessage Create(HttpRequestMessage request, HttpResponseMessage response)
        {
            ObjectContent content;
            if (!response.TryGetContent(out content) || content == null)
            {
                return response;
            }

            var links = _configurationProvider.GetLinksFor(content.Value.GetType(), content.Value);

            ObjectContent objectContent;
            switch (_configurationProvider.GetResponseStyle())
            {
                case ResponseStyle.JsonApi:
                    var jsonApiResponse = JsonApiResponseHelper.Ok(request, ((ObjectContent)response.Content).Value, links, _configurationProvider.GetNullValueHandling());
                    objectContent = CreateObjectContent(response, jsonApiResponse);
                    break;

                default:
                    var hateoasResponse = HateoasResponseHelper.CreateResponseContent(request, ((ObjectContent)response.Content).Value, links);
                    objectContent = CreateObjectContent(response, hateoasResponse);
                    break;
            }

            response.Content = objectContent;
            return response;
        }

        private static ObjectContent CreateObjectContent<TModel>(HttpResponseMessage response, TModel model)
        {
            return new ObjectContent<TModel>(model, ((ObjectContent)response.Content).Formatter);
        }
    }
}