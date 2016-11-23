using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Web.Http;

namespace FluentHateoas.Handling
{
    public class HttpConfigurationWrapper : IHttpConfiguration
    {
        public HttpConfiguration HttpConfiguration { get; }

        public HttpConfigurationWrapper(HttpConfiguration httpConfiguration)
        {
            HttpConfiguration = httpConfiguration;
        }

        public Collection<DelegatingHandler> MessageHandlers => HttpConfiguration.MessageHandlers;
        public ConcurrentDictionary<object, object> Properties => HttpConfiguration.Properties;
    }
}