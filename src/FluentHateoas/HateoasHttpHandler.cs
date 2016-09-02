using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace FluentHateoas
{
    using System.Web.Http;

    using FluentHateoas.Contracts;

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

    public class HateOasConfiguration : IHateOasConfiguration
    {
        private readonly HttpConfiguration _configuration;

        private readonly IHateoasContainer _container;

        public HateOasConfiguration(HttpConfiguration configuration, IHateoasContainer container)
        {
            _configuration = configuration;
            _container = container;
        }

    }

    public interface IHateOasConfiguration
    {
    }
}