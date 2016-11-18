using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web.Http;
using FluentHateoasTest.Assets.Model;

namespace FluentHateoasTest.Assets.Controllers
{
    [ExcludeFromCodeCoverage]
    public class EngineController : ApiController
    {
        public IEnumerable<Engine> Get()
        {
            return new List<Engine>().Select(p => new Engine());
        }

        [Route("{firstname}")]
        public Engine Get(string firstname)
        {
            return new Engine();
        }

        [HttpPost]
        public Engine Post(CreateCarRequest request)
        {
            return new Engine();
        }

        [HttpPut]
        public Engine Put(UpdateCarRequest request)
        {
            return new Engine();
        }

        [HttpDelete]
        public void Delete() { }
    }
}