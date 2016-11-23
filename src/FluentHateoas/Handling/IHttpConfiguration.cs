using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Web.Http;

namespace FluentHateoas.Handling
{
    public interface IHttpConfiguration
    {
        HttpConfiguration HttpConfiguration { get; }

        Collection<DelegatingHandler> MessageHandlers { get; }
        ConcurrentDictionary<object, object> Properties { get; }
    }
}