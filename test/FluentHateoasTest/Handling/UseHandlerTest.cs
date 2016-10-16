using FluentAssertions;
using FluentHateoas.Builder.Handlers;
using FluentHateoas.Registration;
using FluentHateoasTest.Assets.Controllers;
using FluentHateoasTest.Assets.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentHateoasTest.Handling
{
    [TestClass]
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
            Container
                .Register<Person>("create", p => p.Id)
                .Get<PersonController>();

            var registration = Container.GetRegistration<Person>("create");
            Handler.CanProcess(registration, LinkBuilder).Should().BeTrue();
        }

        [TestMethod]
        public void UseHandlerShouldNotProcessWhenInvalid()
        {
            Container
                .Register<Person>("create");

            var registration = Container.GetRegistration<Person>("create");
            Handler.CanProcess(registration, LinkBuilder).Should().BeFalse();
        }

        [TestMethod]
        public void UseHandlerShouldRegisterActionWhenGiven()
        {
            Container
                .Register<Person>("parents")
                .Get<PersonController>(p => p.GetParents);

            var registration = Container.GetRegistration<Person>("parents");

            Handler.CanProcess(registration, LinkBuilder).Should().BeTrue();
            Handler.Process(registration, LinkBuilder, Person);

            LinkBuilder.Controller.Should().NotBeNull();
            LinkBuilder.Controller.Should().Be(typeof(PersonController));
            LinkBuilder.Action.Should().NotBeNull();
            LinkBuilder.Action.Name.Should().Be("GetParents");
        }

        [TestMethod]
        public void UseHandlerShouldChooseMethodBasedOnAvailableArguments()
        {
            Container
                .Register<Person>("list")
                .Get<PersonController>();

            var registration = Container.GetRegistration<Person>("list");

            Handler.CanProcess(registration, LinkBuilder).Should().BeTrue();
            Handler.Process(registration, LinkBuilder, Person);

            LinkBuilder.Controller.Should().NotBeNull();
            LinkBuilder.Controller.Should().Be(typeof(PersonController));
            LinkBuilder.Action.Should().NotBeNull();
            LinkBuilder.Action.Name.Should().Be("Get");
            LinkBuilder.Arguments.Count.Should().Be(0);
        }
    }
}