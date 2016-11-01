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
    public class CommandHandlerTest : BaseHandlerTest<CommandHandler>
    {
        [TestInitialize]
        public void Initialize()
        {
            Handler = new CommandHandler();
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
            Handler.CanProcess(registrationMock.Object, LinkBuilder).Should().BeTrue();

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
            var canProcess = Handler.CanProcess(registration, LinkBuilder);

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

            LinkBuilderMock.SetupSet(lb => lb.Command = It.Is<IHateoasCommand>(comm => (relation + "-command").Equals(comm.Name)));

            // act
            Handler.ProcessInternal(registration, LinkBuilder, Person);

            // assert
            registrationMock.VerifyGet(r => r.Relation, Times.Once);
            expression.VerifyGet(e => e.Command, Times.Exactly(2));
            LinkBuilderMock.VerifySet(lb => lb.Command = It.IsAny<IHateoasCommand>(), Times.Once);
        }
    }
}