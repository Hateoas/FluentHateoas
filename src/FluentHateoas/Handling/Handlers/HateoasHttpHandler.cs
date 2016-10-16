namespace FluentHateoas.Handling.Handlers
{
    public class HateoasHttpHandler : System.Net.Http.DelegatingHandler
    {
        private readonly IResponseProvider _responseProvider;

        public HateoasHttpHandler(IResponseProvider responseProvider)
        {
            _responseProvider = responseProvider;
        }

        protected override async System.Threading.Tasks.Task<System.Net.Http.HttpResponseMessage> SendAsync(System.Net.Http.HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);
            var result = _responseProvider.Create(request, response); // TODO Async?!

            return result;
        }
    }

    // TODO HateoasConfiguration already exists, a bit confusing ;)
    //public class HateOasConfiguration : IHateOasConfiguration
    //{
    //    private readonly HttpConfiguration _configuration;

    //    private readonly IHateoasContainer _container;

    //    public HateOasConfiguration(HttpConfiguration configuration, IHateoasContainer container)
    //    {
    //        _configuration = configuration;
    //        _container = container;
    //    }

    //}

    //public interface IHateOasConfiguration
    //{
    //}
}