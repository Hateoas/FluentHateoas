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
    public class TemplateArgumentsProcessorTest
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
        public void CanProcessShouldReturnFalseIfTemplateParametersIsNull()
        {
            // arrange
            var registrationMock = new Mock<IHateoasRegistration<Person>>();
            var expression = new Mock<IHateoasExpression<Person>>();
            expression.SetupGet(e => e.TemplateParameters).Returns(default(IEnumerable<LambdaExpression>));
            registrationMock.SetupGet(r => r.Expression).Returns(expression.Object);

            // act & assert
            new TemplateArgumentsProcessor().CanProcess(registrationMock.Object, default(ILinkBuilder)).Should().BeFalse();
        }

        [TestMethod]
        public void CanProcessShouldReturnFalseIfTemplateParametersHasNoItems()
        {
            // arrange
            var registrationMock = new Mock<IHateoasRegistration<Person>>();
            var expression = new Mock<IHateoasExpression<Person>>();
            expression.SetupGet(e => e.TemplateParameters).Returns(new List<LambdaExpression>());
            registrationMock.SetupGet(r => r.Expression).Returns(expression.Object);

            // act & assert
            new TemplateArgumentsProcessor().CanProcess(registrationMock.Object, default(ILinkBuilder)).Should().BeFalse();
        }

        [TestMethod]
        public void ProcessShouldAddIdToLinkBuilderUsingTemplateParameters()
        {
            // arrange
            var expressionMock = new Mock<IHateoasExpression<Person>>(MockBehavior.Strict);
            expressionMock.SetupGet(e => e.TemplateParameters).Returns(new Expression<Func<Person, object>>[] { p => p.Id });

            var registrationMock = new Mock<IHateoasRegistration<Person>>(MockBehavior.Strict);
            registrationMock.SetupGet(r => r.Expression).Returns(expressionMock.Object);
            registrationMock.SetupGet(r => r.IsCollection).Returns(false);

            var linkBuilderArgsDic = new Dictionary<string, Argument>();
            var argumentsMock = new Mock<IDictionary<string, Argument>>(MockBehavior.Strict);
            argumentsMock.Setup(a => a.Add(It.IsAny<string>(), It.IsAny<Argument>())).Callback((string key, Argument value) => linkBuilderArgsDic.Add(key, value));
            argumentsMock.Setup(a => a.GetEnumerator()).Returns(() => linkBuilderArgsDic.GetEnumerator());

            var linkBuilderMock = new Mock<ILinkBuilder>(MockBehavior.Strict);
            linkBuilderMock.SetupGet(lb => lb.Arguments).Returns(argumentsMock.Object);

            // act
            new TemplateArgumentsProcessor().Process(registrationMock.Object, linkBuilderMock.Object, _person);

            // assert
            registrationMock.VerifyGet(r => r.Expression, Times.Once);
            linkBuilderMock.VerifyGet(lb => lb.Arguments, Times.Exactly(2));
            argumentsMock.Verify(a => a.Add("id", It.Is<Argument>(arg => "{id}".Equals(arg.Value))), Times.Once);
        }

        [TestMethod]
        public void ProcessShouldAddKeyedParameterToLinkBuilderWhenArgumentsAroundAlreadyUsingTemplateParameters()
        {
            // arrange
            var expressionMock = new Mock<IHateoasExpression<Person>>(MockBehavior.Strict);
            expressionMock.SetupGet(e => e.TemplateParameters).Returns(new Expression<Func<Person, object>>[] { p => p.Id });

            var registrationMock = new Mock<IHateoasRegistration<Person>>(MockBehavior.Strict);
            registrationMock.SetupGet(r => r.Expression).Returns(expressionMock.Object);
            registrationMock.SetupGet(r => r.IsCollection).Returns(false);

            var linkBuilderArgsDic = new Dictionary<string, Argument> { { "id", new Argument() }};
            var argumentsMock = new Mock<IDictionary<string, Argument>>(MockBehavior.Strict);
            argumentsMock.Setup(a => a.Add(It.IsAny<string>(), It.IsAny<Argument>())).Callback((string key, Argument value) => linkBuilderArgsDic.Add(key, value));
            argumentsMock.Setup(a => a.GetEnumerator()).Returns(() => linkBuilderArgsDic.GetEnumerator());

            var linkBuilderMock = new Mock<ILinkBuilder>(MockBehavior.Strict);
            linkBuilderMock.SetupGet(lb => lb.Arguments).Returns(argumentsMock.Object);

            // act
            new TemplateArgumentsProcessor().Process(registrationMock.Object, linkBuilderMock.Object, _person);

            // assert
            registrationMock.VerifyGet(r => r.Expression, Times.Once);
            linkBuilderMock.VerifyGet(lb => lb.Arguments, Times.Exactly(2));
            argumentsMock.Verify(a => a.Add("personId", It.Is<Argument>(arg => "{personId}".Equals(arg.Value))), Times.Once);
        }
    }
}