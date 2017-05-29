using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace FluentHateoas.Handling.Handlers
{
    public class HateoasHttpHandler : DelegatingHandler
    {
        private readonly IResponseProvider _responseProvider;
        private readonly IList<IMessageSerializer> _messageSerializers;

        public HateoasHttpHandler(IResponseProvider responseProvider, IList<IMessageSerializer> messageSerializers)
        {
            _responseProvider = responseProvider;
            _messageSerializers = messageSerializers;
        }

        protected override async System.Threading.Tasks.Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            var serializer = _messageSerializers.FirstOrDefault(p => p.HandlesContentType(HttpContext.Current.Request.ContentType)) ?? new DefaultMessageSerializer();

            request = serializer.OnRequest(request, cancellationToken);
            var response = await base.SendAsync(request, cancellationToken);
            var links = _responseProvider.CreateLinks(response); // TODO Async?! Why?!

            return serializer.OnResponse(request, response, ((ObjectContent)response.Content).Value, links);
        }   
    }
}