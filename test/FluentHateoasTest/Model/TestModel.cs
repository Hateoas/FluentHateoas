using System;

namespace FluentHateoasTest.Model
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http.Controllers;

    public class TestModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class TestModelController : IHttpController
    {
        public IEnumerable<TestModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public TestModel GetSingle(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<HttpResponseMessage> ExecuteAsync(HttpControllerContext controllerContext, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}