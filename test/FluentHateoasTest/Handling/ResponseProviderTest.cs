using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Dependencies;
using System.Web.Http.Hosting;
using FluentHateoas.Handling;
using FluentHateoas.Registration;
using FluentHateoasTest.Assets.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FluentHateoasTest.Handling
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ResponseProviderTest
    {
        [TestMethod]
        public void CreateShouldCreateResponse()
        {
            // arrange
            var person = new Person();
            var configuration = new HttpConfiguration();

            var configurationProviderMock = new Mock<IConfigurationProvider>(MockBehavior.Strict);
            configurationProviderMock.Setup(cp => cp.GetLinksFor(typeof(Person), person)).Returns(new List<IHateoasLink>());

            var responseProvider = new ResponseProvider(configurationProviderMock.Object);

            var request = new HttpRequestMessage();
            request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, configuration);
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ObjectContent(person.GetType(), person, new BsonMediaTypeFormatter(), "application/json")
            };

            // act
            var hateoasResponse = responseProvider.Create(request, response);

            // assert
            var hateoasContent = (ObjectContent<HateOasResponse>)hateoasResponse.Content;
            var hateoasContentValue = (HateOasResponse) hateoasContent.Value;

            Assert.AreEqual(person, hateoasContentValue.Data);
            Assert.AreEqual(0, hateoasContentValue.Links.Count());
            Assert.AreEqual(0, hateoasContentValue.Commands.Count());
        }
    }
}