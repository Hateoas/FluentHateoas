using System;
using System.Diagnostics.CodeAnalysis;
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
    public class ArgumentHandlerTest
    {
        private Mock<ILinkBuilder> _linkBuilderMock;
        private ArgumentHandler _handler;

        private Mock<IArgumentProcessor> _idFromExpressionProcessorMock;
        private Mock<IArgumentProcessor> _argumentsDefinitionsProcessorMock;
        private Mock<IArgumentProcessor> _templateArgumentsProcessorMock;
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

            _idFromExpressionProcessorMock = new Mock<IArgumentProcessor>(MockBehavior.Strict);
            _argumentsDefinitionsProcessorMock = new Mock<IArgumentProcessor>(MockBehavior.Strict);
            _templateArgumentsProcessorMock = new Mock<IArgumentProcessor>(MockBehavior.Strict);

            _linkBuilderMock = new Mock<ILinkBuilder>(MockBehavior.Strict);

            _handler = new ArgumentHandler(
                _idFromExpressionProcessorMock.Object,
                _argumentsDefinitionsProcessorMock.Object,
                _templateArgumentsProcessorMock.Object);
        }

        [TestMethod]
        public void CanProcessShouldBeTrueWhenAllPrerequisitesHaveBeenMet()
        {
            // arrange
            var registrationMock = new Mock<IHateoasRegistration<Person>>(MockBehavior.Strict);

            _argumentsDefinitionsProcessorMock.Setup(p => p.CanProcess(It.IsAny<IHateoasRegistration<Person>>(), It.IsAny<ILinkBuilder>())).Returns(true);
            _templateArgumentsProcessorMock.Setup(p => p.CanProcess(It.IsAny<IHateoasRegistration<Person>>(), It.IsAny<ILinkBuilder>())).Returns(true);
            _idFromExpressionProcessorMock.Setup(p => p.CanProcess(It.IsAny<IHateoasRegistration<Person>>(), It.IsAny<ILinkBuilder>())).Returns(true);

            // act & assert
            _handler.CanProcess(registrationMock.Object, _linkBuilderMock.Object).Should().BeTrue();
        }

        [TestMethod]
        public void CanProcessShouldReturnFalseIfNoProcessorCanProcess()
        {
            // arrange
            var registrationMock = new Mock<IHateoasRegistration<Person>>();

            _argumentsDefinitionsProcessorMock.Setup(p => p.CanProcess(It.IsAny<IHateoasRegistration<Person>>(), It.IsAny<ILinkBuilder>())).Returns(false);
            _templateArgumentsProcessorMock.Setup(p => p.CanProcess(It.IsAny<IHateoasRegistration<Person>>(), It.IsAny<ILinkBuilder>())).Returns(false);
            _idFromExpressionProcessorMock.Setup(p => p.CanProcess(It.IsAny<IHateoasRegistration<Person>>(), It.IsAny<ILinkBuilder>())).Returns(false);

            // act & assert
            _handler.CanProcess(registrationMock.Object, null).Should().BeFalse();
        }

        [TestMethod]
        public void ProcessInternalShouldNotProcessUsingArgumentDefinitionsWhenIdFromExpressionPresent()
        {
            // arrange
            var registrationMock = new Mock<IHateoasRegistration<Person>>();

            _idFromExpressionProcessorMock.Setup(p => p.Process(It.IsAny<IHateoasRegistration<Person>>(), It.IsAny<ILinkBuilder>(), It.IsAny<object>())).Returns(true);
            _templateArgumentsProcessorMock.Setup(p => p.Process(It.IsAny<IHateoasRegistration<Person>>(), It.IsAny<ILinkBuilder>(), It.IsAny<object>())).Returns(true);

            // act
            _handler.ProcessInternal(registrationMock.Object, _linkBuilderMock.Object, _person);

            // assert
            _idFromExpressionProcessorMock.Verify(p => p.Process(registrationMock.Object, _linkBuilderMock.Object, _person), Times.Once);
            _templateArgumentsProcessorMock.Verify(p => p.Process(registrationMock.Object, _linkBuilderMock.Object, _person), Times.Once);
        }

        [TestMethod]
        public void ProcessInternalShouldProcessUsingArgumentDefinitionsWhenNoIdFromExpressionPresent()
        {
            // arrange
            var registrationMock = new Mock<IHateoasRegistration<Person>>();

            _idFromExpressionProcessorMock.Setup(p => p.Process(It.IsAny<IHateoasRegistration<Person>>(), It.IsAny<ILinkBuilder>(), It.IsAny<object>())).Returns(false);
            _argumentsDefinitionsProcessorMock.Setup(p => p.Process(It.IsAny<IHateoasRegistration<Person>>(), It.IsAny<ILinkBuilder>(), It.IsAny<object>())).Returns(true);
            _templateArgumentsProcessorMock.Setup(p => p.Process(It.IsAny<IHateoasRegistration<Person>>(), It.IsAny<ILinkBuilder>(), It.IsAny<object>())).Returns(true);

            // act
            _handler.ProcessInternal(registrationMock.Object, _linkBuilderMock.Object, _person);

            // assert
            _idFromExpressionProcessorMock.Verify(p => p.Process(registrationMock.Object, _linkBuilderMock.Object, _person), Times.Once);
            _argumentsDefinitionsProcessorMock.Verify(p => p.Process(registrationMock.Object, _linkBuilderMock.Object, _person), Times.Once);
            _templateArgumentsProcessorMock.Verify(p => p.Process(registrationMock.Object, _linkBuilderMock.Object, _person), Times.Once);
        }
    }
}