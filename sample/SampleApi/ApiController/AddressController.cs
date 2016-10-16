using System;
using System.Web.Http;

namespace SampleApi.ApiController
{
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    [RoutePrefix("address")]
    public class AddressController : System.Web.Http.ApiController
    {
        [Route("{id}/{houseId}")]
        public object Get(Guid id, Guid houseId)
        {
            return new object();
        }
    }
}