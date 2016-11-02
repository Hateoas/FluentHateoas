using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Net.Http;
using FluentAssertions;
using FluentHateoas.Builder.Handlers;
using FluentHateoas.Builder.Model;
using FluentHateoas.Handling;
using FluentHateoas.Interfaces;
using FluentHateoasTest.Assets.Controllers;
using FluentHateoasTest.Assets.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FluentHateoasTest.Handling
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class UseHandlerTest
    {
        private Person _person;
        private Mock<ILinkBuilder> _linkBuilderMock;
        private UseHandler _handler;

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
            _handler = new UseHandler();
        }

        [TestMethod]
        public void UseHandlerShouldProcessWhenValid()
        {
            // arrange
            var expressionMock = new Mock<IHateoasExpression<Person>>(MockBehavior.Strict);
            expressionMock.SetupGet(e => e.Controller).Returns(typeof(PersonController));
            Expression<Func<Person, object>> argumentDefinitionExpression = p => p.Id;
            expressionMock.SetupGet(e => e.Action).Returns(argumentDefinitionExpression);
            expressionMock.SetupGet(e => e.HttpMethod).Returns(HttpMethod.Get);

            var registrationMock = new Mock<IHateoasRegistration<Person>>(MockBehavior.Strict);
            registrationMock.SetupGet(r => r.Expression).Returns(expressionMock.Object);

            // act & assert
            _handler.CanProcess(registrationMock.Object, _linkBuilderMock.Object).Should().BeTrue();
        }

        [TestMethod]
        public void Use_handlerShouldNotProcessWhenInvalid()
        {
            // arrange
            var registrationMock = new Mock<IHateoasRegistration<Person>>();
            var expression = new Mock<IHateoasExpression<Person>>();
            expression.SetupGet(e => e.Controller).Returns(typeof(PersonController));
            registrationMock.SetupGet(r => r.Expression).Returns(expression.Object);
            var registration = registrationMock.Object;

            // act & assert
            _handler.CanProcess(registration, _linkBuilderMock.Object).Should().BeFalse();
        }

        [TestMethod]
        [Ignore]
        public void Use_handlerShouldRegisterActionWhenGiven()
        {
            // arrange
            //var registration = GetRegistration<Person, PersonController>(p => p.GetParents);

            // act
            //_handler.CanProcess(registration, _linkBuilderMock.Object).Should().BeTrue();
            //_handler.Process(registration, _linkBuilderMock.Object, Person);

            // assert
            _linkBuilderMock.Object.Controller.Should().NotBeNull();
            _linkBuilderMock.Object.Controller.Should().Be(typeof(PersonController));
            _linkBuilderMock.Object.Action.Should().NotBeNull();
            _linkBuilderMock.Object.Action.Name.Should().Be("GetParents");
        }

        [TestMethod]
        public void CanProcessShouldBeTrueWhenRegistrationIsComplete()
        {
            // arrange
            var expressionMock = new Mock<IHateoasExpression<Person>>(MockBehavior.Strict);
            expressionMock.SetupGet(e => e.Controller).Returns(typeof(PersonController));
            expressionMock.SetupGet(e => e.Action).Returns(default(LambdaExpression));
            expressionMock.SetupGet(e => e.HttpMethod).Returns(HttpMethod.Get);

            var registrationMock = new Mock<IHateoasRegistration<Person>>(MockBehavior.Strict);
            registrationMock.SetupGet(r => r.Expression).Returns(expressionMock.Object);

            // act
            var canProcess = _handler.CanProcess(registrationMock.Object, _linkBuilderMock.Object);

            // assert
            canProcess.Should().BeTrue();
        }

        [TestMethod]
        public void ProcessInternalShouldChooseMethodBasedOnAvailableArguments()
        {
            // arrange
            var expressionMock = new Mock<IHateoasExpression<Person>>(MockBehavior.Strict);
            expressionMock.SetupGet(e => e.Controller).Returns(typeof(PersonController));
            expressionMock.SetupGet(e => e.Action).Returns(default(LambdaExpression));
            expressionMock.SetupGet(e => e.HttpMethod).Returns(HttpMethod.Get);

            var registrationMock = new Mock<IHateoasRegistration<Person>>(MockBehavior.Strict);
            registrationMock.SetupGet(r => r.Expression).Returns(expressionMock.Object);

            var getAllMethod = typeof(PersonController).GetMethod("Get", new Type[0]);
            var argumentsMock = new Mock<IDictionary<string, Argument>>(MockBehavior.Strict);
            var enumeratorMock = new Mock<IEnumerator<KeyValuePair<string, Argument>>>(MockBehavior.Loose);

            argumentsMock.SetupGet(a => a.Count).Returns(0);
            argumentsMock.Setup(a => a.GetEnumerator()).Returns(enumeratorMock.Object);

            _linkBuilderMock.SetupGet(lb => lb.Arguments).Returns(argumentsMock.Object);
            _linkBuilderMock.SetupSet(lb => lb.Controller = typeof(PersonController));
            _linkBuilderMock.SetupGet(lb => lb.Relation).Returns("self");
            _linkBuilderMock.SetupGet(lb => lb.Method).Returns(HttpMethod.Get);
            _linkBuilderMock.SetupSet(lb => lb.Action = getAllMethod);

            // act
            _handler.ProcessInternal(registrationMock.Object, _linkBuilderMock.Object, _person);

            // assert
            _linkBuilderMock.VerifyGet(lb => lb.Arguments, Times.Once);
            argumentsMock.Verify(a => a.GetEnumerator(), Times.Once);
            _linkBuilderMock.VerifySet(lb => lb.Controller = typeof(PersonController), Times.Once());
            _linkBuilderMock.VerifyGet(lb => lb.Relation, Times.Once());
            _linkBuilderMock.VerifyGet(lb => lb.Method, Times.Once);
            _linkBuilderMock.VerifySet(lb => lb.Action = getAllMethod, Times.Once());
        }
    }
}