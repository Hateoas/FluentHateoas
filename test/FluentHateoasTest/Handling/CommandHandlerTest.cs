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
    public class CommandHandlerTest
    {
        private CommandHandler _handler;
        private HateoasConfiguration _configuration;
        private TestContainer _container;
        private LinkBuilder _linkBuilder;
        private Person _person;

        [TestInitialize]
        public void Initialize()
        {
            _person = new Person
            {
                Id = Guid.Parse("7AEC12CD-FD43-49DD-A2AB-3CDD19A3A5F4"),
                Birthday = new DateTimeOffset(new DateTime(1980, 1, 1)),
                Firstname = "John",
                Lastname = "Doe"
            };

            _configuration = new HateoasConfiguration();
            _container = new TestContainer(_configuration);

            _handler = new CommandHandler();
            _linkBuilder = new LinkBuilder();
        }

        [TestMethod]
        public void HandlerShouldProcessWithCommand()
        {
            _container
                .Register<Person>("create")
                .Post<PersonController>()
                .WithCommand<CreatePersonRequest>();

            var registration = _container.GetRegistration<Person>("create");
            _handler.CanProcess(registration, _linkBuilder).Should().BeTrue();
            _handler.Process(registration, _linkBuilder, _person);

            _linkBuilder.Command.Should().NotBeNull();
        }

        [TestMethod]
        public void HandlerShouldNotProcessWithoutCommand()
        {
            _container
                .Register<Person>("create")
                .Post<PersonController>();

            var registration = _container.GetRegistration<Person>("create");
            _handler.CanProcess(registration, _linkBuilder).Should().BeFalse();
        }
    }
}