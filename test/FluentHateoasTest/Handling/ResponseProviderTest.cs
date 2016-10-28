using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
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
        //[TestMethod]
        //public void CreateShouldCreateResponse()
        //{
        //    // arrange
        //    var configuration = new HttpConfiguration();

        //    var authorizationProvider = new Mock<IAuthorizationProvider>();
        //    var dependencyResolver = new Mock<IDependencyResolver>();
        //    var linkFactory = new LinkFactory(authorizationProvider.Object, dependencyResolver.Object);

        //    var configurationProviderMock = new ConfigurationProvider(configuration, linkFactory);
        //    var rp = new ResponseProvider(configurationProviderMock);
        //    //var rp = new ResponseProvider(configurationProviderMock.Object);

        //    var person = new Person();
        //    var request = new HttpRequestMessage(HttpMethod.Get, "http://test");
        //    var response = new HttpResponseMessage(HttpStatusCode.OK)
        //    {
        //        Content = new ObjectContent(person.GetType(), person, new BsonMediaTypeFormatter(), "text/json")
        //    };

        //    // act
        //    rp.Create(request, response);

        //    // assert
        //}
    }
}