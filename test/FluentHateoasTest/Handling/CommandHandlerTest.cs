using System;
using FluentAssertions;
using FluentHateoas.Builder.Handlers;
using FluentHateoas.Handling;
using FluentHateoas.Registration;
using FluentHateoasTest.Assets;
using FluentHateoasTest.Assets.Controllers;
using FluentHateoasTest.Assets.Model;
using FluentHateoasTest.Factories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentHateoasTest.Handling
{
    [TestClass]
    public class CommandHandlerTest : BaseHandlerTest<CommandHandler>
    {
        [TestMethod]
        public void HandlerShouldProcessWithCommand()
        {
            Container
                .Register<Person>("create")
                .Post<PersonController>()
                .WithCommand<CreatePersonRequest>();

            var registration = Container.GetRegistration<Person>("create");
            Handler.CanProcess(registration, LinkBuilder).Should().BeTrue();
            Handler.Process(registration, LinkBuilder, Person);

            LinkBuilder.Command.Should().NotBeNull();
        }

        [TestMethod]
        public void HandlerShouldNotProcessWithoutCommand()
        {
            Container
                .Register<Person>("create")
                .Post<PersonController>();

            var registration = Container.GetRegistration<Person>("create");
            Handler.CanProcess(registration, LinkBuilder).Should().BeFalse();
        }
    }
}