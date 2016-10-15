using System;
using System.Linq.Expressions;
using System.Web.Http;
using FluentAssertions;
using FluentHateoas.Builder.Handlers;
using FluentHateoas.Handling;
using FluentHateoas.Handling.Handlers;
using FluentHateoas.Registration;
using FluentHateoasTest.Assets.Controllers;
using FluentHateoasTest.Assets.Model;
using FluentHateoasTest.Factories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FluentHateoasTest.Handling
{
    [TestClass]
    public class RelationNameHandlerTest
    {
        private RelationHandler _handler;
        private Person _object;
        private LinkBuilder _linkBuilder;

        [TestInitialize]
        public void Initialize()
        {
            _handler = new RelationHandler();

            _object = new Person { };
            _linkBuilder = new LinkBuilder();
        }

        [TestMethod]
        public void RelationNameShouldBeUsed()
        {
            var relation = "some-relation";
            var registration = RegistrationFactory.Create<Person>(relation, false);

            var result = _handler.Process(registration, _linkBuilder, _object);

            result.Relation.Should().Be(relation);
        }

        [TestMethod]
        public void EmptyStringRelationNameCantBeProcessed()
        {
            var registration = RegistrationFactory.Create<Person>("", false);

            _handler.CanProcess(registration, _linkBuilder).Should().BeFalse();
        }

        [TestMethod]
        public void NullStringRelationNameCantBeProcessed()
        {
            var registration = RegistrationFactory.Create<Person>(null, false);

            _handler.CanProcess(registration, _linkBuilder).Should().BeFalse();
        }
    }
}
