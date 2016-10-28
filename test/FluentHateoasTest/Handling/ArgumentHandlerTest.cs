using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Web.Http.Dependencies;
using FluentAssertions;
using FluentHateoas.Builder.Handlers;
using FluentHateoas.Interfaces;
using FluentHateoasTest.Assets.Controllers;
using FluentHateoasTest.Assets.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FluentHateoasTest.Handling
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ArgumentHandlerTest : BaseHandlerTest<ArgumentHandler>
    {
        [TestInitialize]
        public void Initialize()
        {
            var dependencyResolverMock = new Mock<IDependencyResolver>();
            Handler = new ArgumentHandler(dependencyResolverMock.Object);
        }

        [TestMethod]
        public void HandlerShouldProcessWhenValid()
        {
            // arrange
            var registration = GetRegistration<Person, PersonController>(p => p.Id);

            // act & assert
            Handler.CanProcess(registration, LinkBuilder).Should().BeTrue();
        }

        [TestMethod]
        public void HandlerShouldSetId()
        {
            // arrange
            var registration = GetRegistration<Person, PersonController>(p => p.Id);

            // act
            Handler.Process(registration, LinkBuilder, Person);

            // assert
            LinkBuilder.Arguments["id"].Value.Should().Be(Person.Id);
        }

        [TestMethod]
        public void HandlerShouldNotProcessWhenInvalid()
        {
            // arrange
            var registrationMock = new Mock<IHateoasRegistration<Person>>();
            var expression = new Mock<IHateoasExpression<Person>>();
            expression.SetupGet(e => e.Controller).Returns(typeof(PersonController));
            expression.SetupGet(e => e.HttpMethod).Returns(HttpMethod.Post);
            registrationMock.SetupGet(r => r.Expression).Returns(expression.Object);
            var registration = registrationMock.Object;

            // act & assert
            Handler.CanProcess(registration, LinkBuilder).Should().BeFalse();
        }
    }
}