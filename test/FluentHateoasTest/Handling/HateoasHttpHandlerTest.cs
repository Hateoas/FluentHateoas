using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using FluentHateoas.Handling;
using FluentHateoas.Handling.Handlers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FluentHateoasTest.Handling
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class HateoasHttpHandlerTest
    {
        [TestMethod]
        public async Task HandlerShouldProcessWhenValid()
        {
            // arrange
            var dispatcherMock = new Mock<HttpControllerDispatcher>(/*MockBehavior.Strict, */new HttpConfiguration());

            var responseProviderMock = new Mock<IResponseProvider>(MockBehavior.Strict);
            Expression<Func<IResponseProvider, HttpResponseMessage>> createExpression = rp => rp.Create(It.IsAny<HttpRequestMessage>(), It.IsAny<HttpResponseMessage>());
            responseProviderMock
                .Setup(createExpression)
                .Returns(default(HttpResponseMessage));

            var handler = new HateoasHttpHandlerMock(responseProviderMock.Object)
            {
                InnerHandler = dispatcherMock.Object
            };

            // act
            await handler.SendAsync(new HttpRequestMessage(), new CancellationToken());

            // assert
            responseProviderMock.Verify(createExpression, Times.Once);
        }

        private class HateoasHttpHandlerMock : HateoasHttpHandler
        {
            public HateoasHttpHandlerMock(IResponseProvider responseProvider) : base(responseProvider, new List<IMessageSerializer>())
            {
                
            }

            public new async System.Threading.Tasks.Task<System.Net.Http.HttpResponseMessage> SendAsync(
                System.Net.Http.HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
            {
                return await base.SendAsync(request, cancellationToken);
            }
        }
    }
}