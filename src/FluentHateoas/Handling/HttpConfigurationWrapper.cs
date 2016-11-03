using System.Collections.Concurrent;
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

        public ConcurrentDictionary<object, object> Properties => _httpConfiguration.Properties;
    }

    public interface IHttpConfiguration
    {
        ConcurrentDictionary<object, object> Properties { get; }
    }
}