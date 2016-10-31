using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web.Http;
using FluentHateoasTest.Assets.Model;

namespace FluentHateoasTest.Assets.Controllers
{
    [ExcludeFromCodeCoverage]
    public class CarController : ApiController
    {
        public IEnumerable<Car> Get()
        {
            return new List<Car>().Select(p => new Car());
        }

        public Car Get(Guid id)
        {
            return new Car();
        }

        [HttpPost]
        public Car Post(CreateCarRequest request)
        {
            return new Car();
        }

        [HttpPut]
        public Car Put(UpdateCarRequest request)
        {
            return new Car();
        }

        [HttpDelete]
        public void Delete() { }
    }
}