using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using FluentAssertions;
using FluentHateoas.Builder.Handlers;
using FluentHateoas.Builder.Model;
using FluentHateoas.Handling;
using FluentHateoas.Interfaces;
using FluentHateoasTest.Assets.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FluentHateoasTest.Handling
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ArgumentDefinitionsProcessorTest
    {
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
        }

        [TestMethod]
        public void CanProcessShouldReturnFalseIfArgumentDefinitionsIsNull()
        {
            // arrange
            var registrationMock = new Mock<IHateoasRegistration<Person>>();
            registrationMock.SetupGet(r => r.ArgumentDefinitions).Returns(default(Expression<Func<Person, object>>[]));
            var registration = registrationMock.Object;

            // act & assert
            new ArgumentDefinitionsProcessor().CanProcess(registration, default(ILinkBuilder)).Should().BeFalse();
        }

        [TestMethod]
        public void ProcessShouldAddIdToLinkBuilderUsingArgumentDefinitions()
        {
            // arrange
            var registrationMock = new Mock<IHateoasRegistration<Person>>(MockBehavior.Strict);
            registrationMock.SetupGet(r => r.ArgumentDefinitions).Returns(new Expression<Func<Person, object>>[] { p => p.Id });

            var argumentsMock = new Mock<IDictionary<string, Argument>>(MockBehavior.Strict);
            argumentsMock.Setup(a => a.Add(It.IsAny<string>(), It.IsAny<Argument>()));

            var linkBuilderMock = new Mock<ILinkBuilder>(MockBehavior.Strict);
            linkBuilderMock.SetupGet(lb => lb.Arguments).Returns(argumentsMock.Object);

            // act
            new ArgumentDefinitionsProcessor().Process(registrationMock.Object, linkBuilderMock.Object, _person);

            // assert
            registrationMock.VerifyGet(r => r.ArgumentDefinitions, Times.Exactly(2));
            linkBuilderMock.VerifyGet(lb => lb.Arguments, Times.Once);
            argumentsMock.Verify(a => a.Add("id", It.Is<Argument>(arg => _person.Id.Equals(arg.Value))), Times.Once);
        }

        [TestMethod]
        public void ProcessShouldDoNothingWhenNoArgumentDefinitions()
        {
            // arrange
            var registrationMock = new Mock<IHateoasRegistration<Person>>(MockBehavior.Strict);
            registrationMock.SetupGet(r => r.ArgumentDefinitions).Returns(default(Expression<Func<Person, object>>[]));
            var linkBuilderMock = new Mock<ILinkBuilder>(MockBehavior.Strict);

            // act
            new ArgumentDefinitionsProcessor().Process(registrationMock.Object, linkBuilderMock.Object, _person);

            // assert
            registrationMock.VerifyGet(r => r.ArgumentDefinitions, Times.Once);
        }
    }
}