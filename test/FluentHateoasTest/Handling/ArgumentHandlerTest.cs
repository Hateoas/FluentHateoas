using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Net.Http;
using System.Web.Http.Dependencies;
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
    public class ArgumentHandlerTest : BaseHandlerTest<ArgumentHandler>
    {
        [TestInitialize]
        public void Initialize()
        {
            var dependencyResolverMock = new Mock<IDependencyResolver>();
            Handler = new ArgumentHandler(dependencyResolverMock.Object);

        }

        [TestMethod]
        public void HandlerShouldProcessWhenValid()
        {
            // arrange
            var registrationMock = new Mock<IHateoasRegistration<Person>>(MockBehavior.Strict);
            registrationMock
                .SetupGet(r => r.ArgumentDefinitions)
                .Returns(new Expression<Func<Person, object>>[] { p => p.Id });

            // act & assert
            Handler.CanProcess(registrationMock.Object, LinkBuilder).Should().BeTrue();
        }

        [TestMethod]
        public void HandlerShouldSetId()
        {
            // arrange
            var expressionMock = new Mock<IHateoasExpression<Person>>(MockBehavior.Strict);
            expressionMock.SetupGet(e => e.TemplateParameters).Returns(new List<LambdaExpression>());
            expressionMock.SetupGet(e => e.IdFromExpression).Returns (default(Expression<Func<object, Person, object>>));

            var registrationMock = new Mock<IHateoasRegistration<Person>>(MockBehavior.Strict);
            registrationMock.SetupGet(r => r.ArgumentDefinitions).Returns(new Expression<Func<Person, object>>[] { p => p.Id });
            registrationMock.SetupGet(r => r.Expression).Returns(expressionMock.Object);

            var argumentsMock = new Mock<IDictionary<string, Argument>>(MockBehavior.Strict);
            argumentsMock.Setup(a => a.Add(It.IsAny<string>(), It.IsAny<Argument>()));
            LinkBuilderMock.SetupGet(lb => lb.Arguments).Returns(argumentsMock.Object);

            // act
            Handler.Process(registrationMock.Object, LinkBuilder, Person);

            // assert
            argumentsMock.Verify(a => a.Add("id", It.Is<Argument>(arg => Person.Id.Equals(arg.Value))), Times.Once);
        }

        [TestMethod]
        public void HandlerShouldNotProcessWhenInvalid()
        {
            // arrange
            var registrationMock = new Mock<IHateoasRegistration<Person>>();
            var expression = new Mock<IHateoasExpression<Person>>();
            expression.SetupGet(e => e.Controller).Returns(typeof(PersonController));
            expression.SetupGet(e => e.HttpMethod).Returns(HttpMethod.Post);
            registrationMock.SetupGet(r => r.Expression).Returns(expression.Object);
            var registration = registrationMock.Object;

            // act & assert
            Handler.CanProcess(registration, LinkBuilder).Should().BeFalse();
        }
    }
}