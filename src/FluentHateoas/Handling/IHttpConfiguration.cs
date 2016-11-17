using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Net.Http;

namespace FluentHateoas.Handling
{
    public interface IHttpConfiguration
    {
        Collection<DelegatingHandler> MessageHandlers { get; }
        ConcurrentDictionary<object, object> Properties { get; }
    }
}