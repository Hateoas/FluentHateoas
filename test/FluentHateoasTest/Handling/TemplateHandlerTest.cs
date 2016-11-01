using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using FluentAssertions;
using FluentHateoas.Builder.Handlers;
using FluentHateoas.Interfaces;
using FluentHateoasTest.Assets.Controllers;
using FluentHateoasTest.Assets.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FluentHateoasTest.Handling
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class TemplateHandlerTest : BaseHandlerTest<TemplateHandler>
    {
        [TestInitialize]
        public void Initialize()
        {
            Handler = new TemplateHandler();
        }

        [TestMethod]
        public void TemplateHandlerShouldAlwaysProcess()
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
        public void HandlerShouldSetTemplateFlag()
        {
            // arrange
            var expressionMock = new Mock<IHateoasExpression<Person>>();
            expressionMock.SetupGet(e => e.Template).Returns(true);

            var registrationMock = new Mock<IHateoasRegistration<Person>>(MockBehavior.Strict);
            registrationMock.SetupGet(r => r.ArgumentDefinitions).Returns(new Expression<Func<Person, object>>[] { p => p.Id });
            registrationMock.SetupGet(r => r.Expression).Returns(expressionMock.Object);

            LinkBuilderMock.SetupSet(lb => lb.IsTemplate = true);

            // act
            Handler.ProcessInternal(registrationMock.Object, LinkBuilder, Person);

            // assert
            LinkBuilderMock.VerifySet(lb => lb.IsTemplate = true, Times.Once);
        }
    }
}