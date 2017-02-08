using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting.Messaging;
using System.Web.Http;
using SampleApi.Model;

namespace SampleApi.ApiController
{
    public class CarController : System.Web.Http.ApiController
    {
        public IEnumerable<Car> Get()
        {
            return new List<Car>();
        }

        public Car GetById(Guid id)
        {
            return new Car();
        }

        [Route("person/{id}/cars")]
        public IEnumerable<Car> GetByPersonId(Guid id)
        {
            return new List<Car>();
        }
    }
}
