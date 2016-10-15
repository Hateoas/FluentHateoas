﻿using FluentAssertions;
using FluentHateoas.Builder.Handlers;
using FluentHateoas.Registration;
using FluentHateoasTest.Assets.Controllers;
using FluentHateoasTest.Assets.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentHateoasTest.Handling
{
    [TestClass]
    public class ArgumentHandlerTest : BaseHandlerTest<ArgumentHandler>
    {
        [TestMethod]
        public void HandlerShouldProcessWhenValid()
        {
            Container
                .Register<Person>("create", p => p.Id)
                .Post<PersonController>();

            var registration = Container.GetRegistration<Person>("create");
            Handler.CanProcess(registration, LinkBuilder).Should().BeTrue();
        }

        [TestMethod]
        public void HandlerShouldSetId()
        {
            Container
                .Register<Person>("create", p => p.Id)
                .Post<PersonController>();

            var registration = Container.GetRegistration<Person>("create");

            Handler.Process(registration, LinkBuilder, Person);
            LinkBuilder.Argument.Should().Be(Person.Id);
        }

        [TestMethod]
        public void HandlerShouldNotProcessWhenInvalid()
        {
            Container
                .Register<Person>("create")
                .Post<PersonController>();

            var registration = Container.GetRegistration<Person>("create");
            Handler.CanProcess(registration, LinkBuilder).Should().BeFalse();
        }
    }
}