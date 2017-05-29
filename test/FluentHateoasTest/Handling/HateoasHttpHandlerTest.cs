using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using FluentHateoas.Handling;
using FluentHateoas.Handling.Handlers;
using FluentHateoasTest.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SampleApi.Data.Model;

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
            var responseProviderMock = new Mock<IResponseProvider>(MockBehavior.Strict);
            Expression<Func<IResponseProvider, IEnumerable<IHateoasLink>>> createExpression = rp => rp.CreateLinks(It.IsAny<HttpResponseMessage>());

            responseProviderMock
                .Setup(createExpression)
                .Returns(new List<IHateoasLink>());

            var handler = new HateoasHttpHandler(responseProviderMock.Object, new List<IMessageSerializer>())
            {
                InnerHandler = new TestHandler((message, token) => new HttpResponseMessage(HttpStatusCode.Accepted)
                {
                    Content = new ObjectContent(typeof(Person), new Person(), new JsonMediaTypeFormatter())
                })
            };
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, "http://hateoas.net");
            var client = new HttpClient(handler);

            // act
            var result = await client.SendAsync(requestMessage);

            // assert
            responseProviderMock.Verify(createExpression, Times.Once);
        }
    }
}