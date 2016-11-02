using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Web.Http.Dependencies;
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
    public class IdFromExpressionProcessorTest
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
        public void CanProcessShouldReturnFalseIfIdFromExpressionIsNull()
        {
            // arrange
            var registrationMock = new Mock<IHateoasRegistration<Person>>(MockBehavior.Strict);
            var expression = new Mock<IHateoasExpression<Person>>(MockBehavior.Strict);
            expression.SetupGet(e => e.IdFromExpression).Returns(default(LambdaExpression));
            registrationMock.SetupGet(r => r.Expression).Returns(expression.Object);

            // act & assert
            new IdFromExpressionProcessor(null)
                .CanProcess(registrationMock.Object, default(ILinkBuilder))
                .Should()
                .BeFalse();
        }

        [TestMethod]
        public void ProcessShouldAddIdToLinkBuilderUsingIdFromExpression()
        {
            // arrange
            var dependencyResolverMock = new Mock<IDependencyResolver>(MockBehavior.Strict);
            dependencyResolverMock.Setup(dr => dr.GetService(It.IsAny<Type>())).Returns(null);

            var expressionMock = new Mock<IHateoasExpression<Person>>(MockBehavior.Strict);
            expressionMock.SetupGet(e => e.IdFromExpression).Returns((Expression<Func<object, Person, object>>)((provider, person) => person.Id));

            var registrationMock = new Mock<IHateoasRegistration<Person>>(MockBehavior.Strict);
            registrationMock.SetupGet(r => r.Expression).Returns(expressionMock.Object);

            var argumentsMock = new Mock<IDictionary<string, Argument>>(MockBehavior.Strict);
            argumentsMock.Setup(a => a.Add(It.IsAny<string>(), It.IsAny<Argument>()));

            var linkBuilderMock = new Mock<ILinkBuilder>(MockBehavior.Strict);
            linkBuilderMock.SetupGet(lb => lb.Arguments).Returns(argumentsMock.Object);

            // act
            new IdFromExpressionProcessor(dependencyResolverMock.Object).Process(registrationMock.Object, linkBuilderMock.Object, _person);

            // assert
            dependencyResolverMock.Verify(dr => dr.GetService(It.Is<Type>(t => t == typeof(object) /* mocked provider type */)), Times.Once);
            expressionMock.VerifyGet(e => e.IdFromExpression, Times.Exactly(3));
            registrationMock.VerifyGet(r=>r.Expression, Times.Exactly(3));
            linkBuilderMock.VerifyGet(lb => lb.Arguments, Times.Once);
            argumentsMock.Verify(a => a.Add("id", It.Is<Argument>(arg => _person.Id.Equals(arg.Value))), Times.Once);
        }

        [TestMethod]
        public void ProcessShouldDoNothingWhenNoIdFromExpression()
        {
            // arrange
            var dependencyResolverMock = new Mock<IDependencyResolver>(MockBehavior.Strict);
            var expressionMock = new Mock<IHateoasExpression<Person>>(MockBehavior.Strict);
            expressionMock.SetupGet(e => e.IdFromExpression).Returns(default(Expression<Func<object, Person, object>>));
            var registrationMock = new Mock<IHateoasRegistration<Person>>(MockBehavior.Strict);
            registrationMock.SetupGet(r => r.Expression).Returns(expressionMock.Object);
            var linkBuilderMock = new Mock<ILinkBuilder>(MockBehavior.Strict);

            // act
            new IdFromExpressionProcessor(dependencyResolverMock.Object).Process(registrationMock.Object, linkBuilderMock.Object, _person);

            // assert
            expressionMock.VerifyGet(e => e.IdFromExpression, Times.Once);
            registrationMock.VerifyGet(r => r.Expression, Times.Once);
        }
    }
}