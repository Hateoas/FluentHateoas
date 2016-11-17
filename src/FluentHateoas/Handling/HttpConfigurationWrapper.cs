using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Web.Http;

namespace FluentHateoas.Handling
{
    public class HttpConfigurationWrapper : IHttpConfiguration
    {
        private readonly HttpConfiguration _httpConfiguration;

        public HttpConfigurationWrapper(HttpConfiguration httpConfiguration)
        {
            _httpConfiguration = httpConfiguration;
        }

        public Collection<DelegatingHandler> MessageHandlers => _httpConfiguration.MessageHandlers;
        public ConcurrentDictionary<object, object> Properties => _httpConfiguration.Properties;
    }
}