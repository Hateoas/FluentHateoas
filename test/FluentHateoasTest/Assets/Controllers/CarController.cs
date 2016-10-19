using System;
using System.Collections.Generic;
using System.Web.Http;
using FluentHateoasTest.Assets.Model;

namespace FluentHateoasTest.Assets.Controllers
{
    public class CarController : ApiController
    {
        public IEnumerable<Car> Get()
        {
            return new List<Car>();
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