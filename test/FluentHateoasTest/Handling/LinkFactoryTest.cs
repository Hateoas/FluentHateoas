using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using FluentAssertions;
using FluentHateoas.Builder.Handlers;
using FluentHateoas.Handling;
using FluentHateoas.Interfaces;
using FluentHateoas.Registration;
using FluentHateoasTest.Assets.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FluentHateoasTest.Handling
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class LinkFactoryTest
    {
        private LinkFactory _factory;

        private Mock<ILinkBuilderFactory> _linkBuilderFactoryMock;
        private Mock<IAuthorizationProvider> _authorizationProviderMock;
        private Mock<IArgumentProcessor> _idFromExpressionProcessorMock;
        private Mock<IArgumentProcessor> _argumentsDefinitionsProcessorMock;
        private Mock<IArgumentProcessor> _templateArgumentsProcessorMock;
        private Mock<IRegistrationLinkHandler> _registrationHandlerMock;

        [TestInitialize]
        public void Initialize()
        {
            _linkBuilderFactoryMock = new Mock<ILinkBuilderFactory>(MockBehavior.Strict);
            _authorizationProviderMock = new Mock<IAuthorizationProvider>(MockBehavior.Strict);
            _idFromExpressionProcessorMock = new Mock<IArgumentProcessor>(MockBehavior.Strict);
            _argumentsDefinitionsProcessorMock = new Mock<IArgumentProcessor>(MockBehavior.Strict);
            _templateArgumentsProcessorMock = new Mock<IArgumentProcessor>(MockBehavior.Strict);
            _registrationHandlerMock = new Mock<IRegistrationLinkHandler>(MockBehavior.Strict);

            _factory = new LinkFactory(
                _linkBuilderFactoryMock.Object,
                _authorizationProviderMock.Object,
                _idFromExpressionProcessorMock.Object,
                _argumentsDefinitionsProcessorMock.Object,
                _templateArgumentsProcessorMock.Object,
                _registrationHandlerMock.Object);
        }

        [TestMethod]
        public void DefaultHandlersShouldReturnDefaultHandlers()
        {
            // arrange
            _factory = new LinkFactory(
                _linkBuilderFactoryMock.Object,
                _authorizationProviderMock.Object,
                _idFromExpressionProcessorMock.Object,
                _argumentsDefinitionsProcessorMock.Object,
                _templateArgumentsProcessorMock.Object);

            // act & assert
            _factory.DefaultHandlers
                .Should().HaveCount(7)
                .And.Contain(h => typeof(RelationHandler) == h.GetType())
                .And.Contain(h => typeof(HttpMethodHandler) == h.GetType())
                .And.Contain(h => typeof(CommandHandler) == h.GetType())
                .And.Contain(h => typeof(ArgumentHandler) == h.GetType())
                .And.Contain(h => typeof(TemplateHandler) == h.GetType())
                .And.Contain(h => typeof(UseHandler) == h.GetType())
                .And.Contain(h => typeof(SuccessHandler) == h.GetType());
        }

        [TestMethod]
        public void CreateLinksShouldCreateLinks()
        {
            // arrange
            Expression<Func<IRegistrationLinkHandler, ILinkBuilder>> processHandlerExp = r => r.Process(
                It.IsAny<IHateoasRegistration<Person>>(),
                It.IsAny<ILinkBuilder>(),
                It.IsAny<object>());
            _registrationHandlerMock
                .Setup(processHandlerExp)
                .Returns((IHateoasRegistration<Person> reg, ILinkBuilder bldr, object obj) => bldr);

            var linkBuilderMock = new Mock<ILinkBuilder>(MockBehavior.Strict);
            linkBuilderMock.SetupGet(lb => lb.Success).Returns(true);
            linkBuilderMock.Setup(lb => lb.Build()).Returns(default(IHateoasLink));

            _linkBuilderFactoryMock
                .Setup(f => f.GetLinkBuilder(It.IsAny<object>()))
                .Returns(linkBuilderMock.Object);

            var registrationMock = new Mock<IHateoasRegistration<Person>>(MockBehavior.Strict);

            var registrations = new List<IHateoasRegistration<Person>> { registrationMock.Object };

            var data = new Person();

            // act & assert
            _factory
                .CreateLinks(registrations, data)
                .Should().HaveCount(1);

            _registrationHandlerMock.Verify(processHandlerExp, Times.Once);
            linkBuilderMock.VerifyGet(lb => lb.Success, Times.Once);
            linkBuilderMock.Verify(lb => lb.Build(), Times.Once);
        }
    }
}