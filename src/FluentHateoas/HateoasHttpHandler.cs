using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace FluentHateoas
{
    public class HateoasHttpHandler : DelegatingHandler
    {
        private readonly IResponseProvider _responseProvider;

        public HateoasHttpHandler(IResponseProvider responseProvider)
        {
            _responseProvider = responseProvider;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);

            var result = _responseProvider.Create(response);

            return result;
        }
    }
}