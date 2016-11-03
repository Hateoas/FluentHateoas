using System;
using System.Collections.Generic;

namespace SampleApi.ApiController
{
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    [System.Web.Http.RoutePrefix("address")]
    public class AddressController : System.Web.Http.ApiController
    {
        public IEnumerable<object> Get()
        {
            return new List<object>();
        }

        [System.Web.Http.Route("{id}/{houseId}")]
        public object Get(Guid id, Guid houseId)
        {
            return new object();
        }
    }
}