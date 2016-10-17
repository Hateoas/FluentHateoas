using System.Net.Http;
using FluentAssertions;
using FluentHateoas.Builder.Handlers;
using FluentHateoas.Interfaces;
using FluentHateoasTest.Assets.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SampleApi.Model;
using Person = FluentHateoasTest.Assets.Model.Person;

namespace FluentHateoasTest.Handling
{
    [TestClass]
    public class CommandHandlerTest : BaseHandlerTest<CommandHandler>
    {
        [TestInitialize]
        public void Initialize()
        {
            Handler = new CommandHandler();
        }

        [TestMethod]
        public void HandlerShouldProcessWithCommand()
        {
            // arrange
            var registrationMock = new Mock<IHateoasRegistration<Person>>();

            var expression = new Mock<IHateoasExpression<Person>>();
            expression.SetupGet(e => e.Controller).Returns(typeof(PersonController));
            expression.SetupGet(e => e.Command).Returns(typeof(PersonPostCommand));
            expression.SetupGet(e => e.HttpMethod).Returns(HttpMethod.Post);

            registrationMock.SetupGet(r => r.Expression).Returns(expression.Object);
            var registration = registrationMock.Object;

            // act
            Handler.CanProcess(registration, LinkBuilder).Should().BeTrue();
            Handler.Process(registration, LinkBuilder, Person);

            // assert
            LinkBuilder.Command.Should().NotBeNull();
        }

        [TestMethod]
        public void HandlerShouldNotProcessWithoutCommand()
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