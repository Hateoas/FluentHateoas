using System;
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
    public class ArgumentDefinitionsProcessorTest
    {
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
    }
}