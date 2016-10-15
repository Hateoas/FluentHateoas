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
    }
}