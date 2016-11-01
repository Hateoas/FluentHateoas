using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using FluentAssertions;
using FluentHateoas.Builder.Handlers;
using FluentHateoas.Builder.Model;
using FluentHateoas.Interfaces;
using FluentHateoasTest.Assets.Controllers;
using FluentHateoasTest.Assets.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FluentHateoasTest.Handling
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class UseHandlerTest : BaseHandlerTest<UseHandler>
    {
        [TestInitialize]
        public void Initialize()
        {
            Handler = new UseHandler();
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
            Handler.CanProcess(registrationMock.Object, LinkBuilder).Should().BeTrue();
        }

        [TestMethod]
        public void UseHandlerShouldNotProcessWhenInvalid()
        {
            // arrange
            var registrationMock = new Mock<IHateoasRegistration<Person>>();
            var expression = new Mock<IHateoasExpression<Person>>();
            expression.SetupGet(e => e.Controller).Returns(typeof(PersonController));
            registrationMock.SetupGet(r => r.Expression).Returns(expression.Object);
            var registration = registrationMock.Object;

            // act & assert
            Handler.CanProcess(registration, LinkBuilder).Should().BeFalse();
        }

        [TestMethod]
        [Ignore]
        public void UseHandlerShouldRegisterActionWhenGiven()
        {
            // arrange
            //var registration = GetRegistration<Person, PersonController>(p => p.GetParents);

            // act
            //Handler.CanProcess(registration, LinkBuilder).Should().BeTrue();
            //Handler.Process(registration, LinkBuilder, Person);

            // assert
            LinkBuilder.Controller.Should().NotBeNull();
            LinkBuilder.Controller.Should().Be(typeof(PersonController));
            LinkBuilder.Action.Should().NotBeNull();
            LinkBuilder.Action.Name.Should().Be("GetParents");
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
            var canProcess = Handler.CanProcess(registrationMock.Object, LinkBuilder);

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

            LinkBuilderMock.SetupGet(lb => lb.Arguments).Returns(argumentsMock.Object);
            LinkBuilderMock.SetupSet(lb => lb.Controller = typeof(PersonController));
            LinkBuilderMock.SetupGet(lb => lb.Relation).Returns("self");
            LinkBuilderMock.SetupGet(lb => lb.Method).Returns(HttpMethod.Get);
            LinkBuilderMock.SetupSet(lb => lb.Action = getAllMethod);

            // act
            Handler.ProcessInternal(registrationMock.Object, LinkBuilder, Person);

            // assert
            LinkBuilderMock.VerifyGet(lb => lb.Arguments, Times.Once);
            argumentsMock.Verify(a => a.GetEnumerator(), Times.Once);
            LinkBuilderMock.VerifySet(lb => lb.Controller = typeof(PersonController), Times.Once());
            LinkBuilderMock.VerifyGet(lb => lb.Relation, Times.Once());
            LinkBuilderMock.VerifyGet(lb => lb.Method, Times.Once);
            LinkBuilderMock.VerifySet(lb => lb.Action = getAllMethod, Times.Once());
        }
    }
}