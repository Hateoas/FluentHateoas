using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FluentHateoas.Handling.Handlers
{
    public class HateoasHttpHandler : System.Net.Http.DelegatingHandler
    {
        private readonly IResponseProvider _responseProvider;
        private readonly IList<IMessageSerializer> _messageSerializers;

        public HateoasHttpHandler(IResponseProvider responseProvider, IList<IMessageSerializer> messageSerializers)
        {
            _responseProvider = responseProvider;
            _messageSerializers = messageSerializers;
        }

        protected override async System.Threading.Tasks.Task<System.Net.Http.HttpResponseMessage> SendAsync(System.Net.Http.HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            var serializer = _messageSerializers.FirstOrDefault(p => p.HandlesContentType(HttpContext.Current.Request.ContentType));

            var response = await base.SendAsync(request, cancellationToken);
            var result = _responseProvider.Create(request, response); // TODO Async?!

            return result;
        }
    }
}