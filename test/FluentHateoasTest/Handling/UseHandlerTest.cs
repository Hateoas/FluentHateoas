using System.Diagnostics.CodeAnalysis;
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
    public class UseHandlerTest : BaseHandlerTest<UseHandler>
    {
        [TestInitialize]
        public void Initialize()
        {
            Handler = new UseHandler();
        }

        [TestMethod]
        public void UseHandlerShouldProcessWhenValid()
        {
            // arrange
            var registration = GetRegistration<Person, PersonController>(p => p.Id);

            // act & assert
            Handler.CanProcess(registration, LinkBuilder).Should().BeTrue();
        }

        [TestMethod]
        public void UseHandlerShouldNotProcessWhenInvalid()
        {
            // arrange
            var registrationMock = new Mock<IHateoasRegistration<Person>>();
            var expression = new Mock<IHateoasExpression<Person>>();
            expression.SetupGet(e => e.Controller).Returns(typeof(PersonController));
            registrationMock.SetupGet(r => r.Expression).Returns(expression.Object);
            var registration = registrationMock.Object;

            // act & assert
            Handler.CanProcess(registration, LinkBuilder).Should().BeFalse();
        }

        [TestMethod]
        [Ignore]
        public void UseHandlerShouldRegisterActionWhenGiven()
        {
            // arrange
            //var registration = GetRegistration<Person, PersonController>(p => p.GetParents);

            // act
            //Handler.CanProcess(registration, LinkBuilder).Should().BeTrue();
            //Handler.Process(registration, LinkBuilder, Person);

            // assert
            LinkBuilder.Controller.Should().NotBeNull();
            LinkBuilder.Controller.Should().Be(typeof(PersonController));
            LinkBuilder.Action.Should().NotBeNull();
            LinkBuilder.Action.Name.Should().Be("GetParents");
        }

        [TestMethod]
        public void UseHandlerShouldChooseMethodBasedOnAvailableArguments()
        {
            // arrange
            var registration = GetRegistration<Person, PersonController>();

            // act
            Handler.CanProcess(registration, LinkBuilder).Should().BeTrue();
            Handler.Process(registration, LinkBuilder, Person);

            // assert
            LinkBuilder.Controller.Should().NotBeNull();
            LinkBuilder.Controller.Should().Be(typeof(PersonController));
            LinkBuilder.Action.Should().NotBeNull();
            LinkBuilder.Action.Name.Should().Be("Get");
            LinkBuilder.Arguments.Count.Should().Be(0);
        }
    }
}