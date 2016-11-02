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
    public class IdFromExpressionProcessorTest
    {
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
    }
}