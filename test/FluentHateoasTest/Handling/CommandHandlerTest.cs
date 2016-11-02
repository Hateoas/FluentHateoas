using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using FluentHateoas.Builder.Handlers;
using FluentHateoas.Handling;
using FluentHateoas.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SampleApi.Model;
using Person = FluentHateoasTest.Assets.Model.Person;

namespace FluentHateoasTest.Handling
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class CommandHandlerTest
    {
        private Person _person;
        private Mock<ILinkBuilder> _linkBuilderMock;
        private CommandHandler _handler;

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
            _handler = new CommandHandler();
        }

        [TestMethod]
        public void CanProcessShouldReturnTrueIfRegistrationExpressionCommandPresent()
        {
            // arrange
            var expression = new Mock<IHateoasExpression<Person>>(MockBehavior.Strict);
            expression.SetupGet(e => e.Command).Returns(typeof(PersonPostCommand));

            var registrationMock = new Mock<IHateoasRegistration<Person>>(MockBehavior.Strict);
            registrationMock.SetupGet(r => r.Expression).Returns(expression.Object);

            // act
            _handler.CanProcess(registrationMock.Object, _linkBuilderMock.Object).Should().BeTrue();

            // assert
            expression.VerifyGet(e => e.Command, Times.Once);
        }

        [TestMethod]
        public void CanProcessShouldReturnFalseIfRegistrationExpressionCommandNotPresent()
        {
            // arrange
            var expression = new Mock<IHateoasExpression<Person>>(MockBehavior.Strict);
            expression.SetupGet(e => e.Command).Returns(default(Type));

            var registrationMock = new Mock<IHateoasRegistration<Person>>(MockBehavior.Strict);
            registrationMock.SetupGet(r => r.Expression).Returns(expression.Object);
            var registration = registrationMock.Object;

            // act
            var canProcess = _handler.CanProcess(registration, _linkBuilderMock.Object);

            // assert
            canProcess.Should().BeFalse();
            expression.VerifyGet(e => e.Command, Times.Once);
        }

        [TestMethod]
        public void ProcessInternalShouldSetLinkBuilderCommand()
        {
            // arrange
            const string relation = "self";
            var registrationMock = new Mock<IHateoasRegistration<Person>>(MockBehavior.Strict);
            registrationMock.SetupGet(r => r.Relation).Returns(relation);

            var expression = new Mock<IHateoasExpression<Person>>(MockBehavior.Strict);
            expression.SetupGet(e => e.Command).Returns(typeof(PersonPostCommand));

            registrationMock.SetupGet(r => r.Expression).Returns(expression.Object);
            var registration = registrationMock.Object;

            _linkBuilderMock.SetupSet(lb => lb.Command = It.Is<IHateoasCommand>(comm => (relation + "-command").Equals(comm.Name)));

            // act
            _handler.ProcessInternal(registration, _linkBuilderMock.Object, _person);

            // assert
            registrationMock.VerifyGet(r => r.Relation, Times.Once);
            expression.VerifyGet(e => e.Command, Times.Exactly(2));
            _linkBuilderMock.VerifySet(lb => lb.Command = It.IsAny<IHateoasCommand>(), Times.Once);
        }
    }
}