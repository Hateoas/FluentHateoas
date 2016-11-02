using System.Collections.Generic;
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
    public class TemplateArgumentsProcessorTest
    {
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
    }
}