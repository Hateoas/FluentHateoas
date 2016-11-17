using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Hosting;
using FluentAssertions;
using FluentHateoas.Handling;
using FluentHateoasTest.Assets.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FluentHateoasTest.Handling
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ResponseProviderTest
    {
        private Mock<IConfigurationProvider> _configurationProviderMock;
        private ResponseProvider _responseProvider;

        [TestInitialize]
        public void TestInitialize()
        {
            _configurationProviderMock = new Mock<IConfigurationProvider>(MockBehavior.Strict);
            _responseProvider = new ResponseProvider(_configurationProviderMock.Object);
        }

        [TestMethod]
        public void CreateShouldCreateResponse()
        {
            // arrange
            var person = new Person();

            _configurationProviderMock.Setup(cp => cp.GetLinksFor(typeof(Person), person)).Returns(new List<IHateoasLink>());

            var request = new HttpRequestMessage();

            var configuration = new HttpConfiguration();
            request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, configuration);
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ObjectContent(person.GetType(), person, new BsonMediaTypeFormatter(), "application/json")
            };

            // act
            var content = _responseProvider
                .Create(request, response).Content;

            // assert
            content.Should().NotBeNull().And.BeOfType<ObjectContent<HateOasResponse>>();
            ((ObjectContent<HateOasResponse>) content).Value
                .Should().BeOfType<HateOasResponse>()
                .And.Match((HateOasResponse r) => person.Equals(r.Data))
                .And.Match((HateOasResponse r) => 0.Equals(r.Links.Count()))
                .And.Match((HateOasResponse r) => 0.Equals(r.Commands.Count()));
        }

        [TestMethod]
        public void CreateShouldReturnOriginalResponseOnNoSuccessReturnCode()
        {
            // arrange
            var request = new HttpRequestMessage();
            var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new ObjectContent(typeof(Person), default(Person), new BsonMediaTypeFormatter(), "application/json")
            };

            // act & assert
            _responseProvider
                .Create(request, response)
                .Should().BeSameAs(response);

        }
    }
}