using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using FluentAssertions;
using FluentHateoas.Builder.Handlers;
using FluentHateoas.Handling;
using FluentHateoas.Interfaces;
using FluentHateoasTest.Assets.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FluentHateoasTest.Handling
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class TemplateHandlerTest
    {
        private Person _person;
        private Mock<ILinkBuilder> _linkBuilderMock;
        private TemplateHandler _handler;

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

            _linkBuilderMock = new Mock<ILinkBuilder>(MockBehavior.Strict);
            _handler = new TemplateHandler();
        }

        [TestMethod]
        public void TemplateHandlerShouldAlwaysProcess()
        {
            // arrange
            var registrationMock = new Mock<IHateoasRegistration<Person>>(MockBehavior.Strict);
            registrationMock
                .SetupGet(r => r.ArgumentDefinitions)
                .Returns(new Expression<Func<Person, object>>[] { p => p.Id });

            // act & assert
            _handler.CanProcess(registrationMock.Object, _linkBuilderMock.Object).Should().BeTrue();
        }

        [TestMethod]
        public void HandlerShouldSetTemplateFlag()
        {
            // arrange
            var expressionMock = new Mock<IHateoasExpression<Person>>();
            expressionMock.SetupGet(e => e.Template).Returns(true);

            var registrationMock = new Mock<IHateoasRegistration<Person>>(MockBehavior.Strict);
            registrationMock.SetupGet(r => r.ArgumentDefinitions).Returns(new Expression<Func<Person, object>>[] { p => p.Id });
            registrationMock.SetupGet(r => r.Expression).Returns(expressionMock.Object);

            _linkBuilderMock.SetupSet(lb => lb.IsTemplate = true);

            // act
            _handler.ProcessInternal(registrationMock.Object, _linkBuilderMock.Object, _person);

            // assert
            _linkBuilderMock.VerifySet(lb => lb.IsTemplate = true, Times.Once);
        }
    }
}