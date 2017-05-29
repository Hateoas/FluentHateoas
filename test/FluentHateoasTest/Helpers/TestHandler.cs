using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace FluentHateoasTest.Helpers
{
    public class TestHandler : DelegatingHandler
    {
        private readonly Func<HttpRequestMessage,
            CancellationToken, Task<HttpResponseMessage>> _handlerFunc;

        public TestHandler()
        {
            _handlerFunc = (r, c) => Return200();
        }

        public TestHandler(Func<HttpRequestMessage, CancellationToken, HttpResponseMessage> handlerFunc)
        {
            _handlerFunc = (r, c) => Task.Factory.StartNew(() => handlerFunc(r, c));
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return _handlerFunc(request, cancellationToken);
        }

        public static Task<HttpResponseMessage> Return200()
        {
            return Task.Factory.StartNew(() => new HttpResponseMessage(HttpStatusCode.OK));
        }
    }
}