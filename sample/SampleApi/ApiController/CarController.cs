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

        [Route("car/{id}")]
        public Car GetById(Guid id)
        {
            return new Car();
        }

        [Route("person/{id}/cars")]
        public IEnumerable<Car> GetByPersonId(Guid id)
        {
            return new List<Car>
            {
                new Car {Id = Guid.Parse("E60F3D19-3847-4380-9B3C-AD9777AC3042"), Name = "Test vehice", PersonId = id}
            };
        }
    }
}
