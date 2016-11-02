using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Net.Http;
using System.Reflection;
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
    public class HttpMethodHandlerTest
    {
        private Person _person;
        private HttpMethodHandler _handler;
        private Mock<IHateoasRegistration<Person>> _registrationMock;
        private Mock<ILinkBuilder> _linkBuilderMock;

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

            _handler = new HttpMethodHandler();
            _registrationMock = new Mock<IHateoasRegistration<Person>>();
            _linkBuilderMock = new Mock<ILinkBuilder>(MockBehavior.Strict);
        }

        [TestMethod]
        public void CanProcessShouldReturnTrue()
        {
            // arrange
            // act & assert
            _handler.CanProcess(_registrationMock.Object, _linkBuilderMock.Object).Should().BeTrue();
        }

        [TestMethod]
        public void ProcessInternalShouldSetLinkBuilderSuccessToTrueWhenActionIsAuthorized()
        {
            // arrange
            var expressionMock = new Mock<IHateoasExpression<Person>>(MockBehavior.Strict);
            expressionMock.SetupGet(e => e.HttpMethod).Returns(HttpMethod.Get);
            _registrationMock.SetupGet(r => r.Expression).Returns(expressionMock.Object);

            _linkBuilderMock.SetupSet(lb => lb.Method = It.IsAny<HttpMethod>());

            // act
            _handler.ProcessInternal(_registrationMock.Object, _linkBuilderMock.Object, _person);

            // assert
            _registrationMock.VerifyGet(r => r.Expression, Times.Once);
            expressionMock.VerifyGet(e => e.HttpMethod, Times.Once);
            _linkBuilderMock.VerifySet(lb => lb.Method = HttpMethod.Get, Times.Once);
        }
    }

    [TestClass]
    [ExcludeFromCodeCoverage]
    public class SuccessHandlerTest
    {
        private Person _person;
        private Mock<IAuthorizationProvider> _authorizationProviderMock;
        private SuccessHandler _handler;
        private Mock<IHateoasRegistration<Person>> _registrationMock;
        private Mock<ILinkBuilder> _linkBuilderMock;

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

            _authorizationProviderMock = new Mock<IAuthorizationProvider>(MockBehavior.Strict);
            _handler = new SuccessHandler(_authorizationProviderMock.Object);
            _registrationMock = new Mock<IHateoasRegistration<Person>>();
            _linkBuilderMock = new Mock<ILinkBuilder>(MockBehavior.Strict);
        }

        [TestMethod]
        public void CanProcessShouldReturnTrue()
        {
            // arrange
            // act & assert
            _handler.CanProcess(_registrationMock.Object, _linkBuilderMock.Object).Should().BeTrue();
        }

        [TestMethod]
        public void ProcessInternalShouldSetLinkBuilderSuccessToTrueWhenActionIsAuthorized()
        {
            // arrange
            _authorizationProviderMock.Setup(ap => ap.IsAuthorized(It.IsAny<MethodInfo>())).Returns(true);
            var actionMock = new Mock<MethodInfo>(MockBehavior.Strict);
            _linkBuilderMock.SetupGet(lb => lb.Action).Returns(actionMock.Object);
            _linkBuilderMock.SetupSet(lb => lb.Success = It.IsAny<bool>());

            // act
            _handler.ProcessInternal(_registrationMock.Object, _linkBuilderMock.Object, _person);

            // assert
            _authorizationProviderMock.Verify(ap=>ap.IsAuthorized(actionMock.Object), Times.Once);
            _linkBuilderMock.VerifyGet(lb=>lb.Action, Times.Exactly(2));
            _linkBuilderMock.VerifySet(lb=>lb.Success = true, Times.Once);
        }

        [TestMethod]
        public void ProcessInternalShouldSetLinkBuilderSuccessToFalseWhenActionNull()
        {
            // arrange
            _linkBuilderMock.SetupGet(lb => lb.Action).Returns(default(MethodInfo));
            _linkBuilderMock.SetupSet(lb => lb.Success = It.IsAny<bool>());

            // act
            _handler.ProcessInternal(_registrationMock.Object, _linkBuilderMock.Object, _person);

            // assert
            _linkBuilderMock.VerifyGet(lb => lb.Action, Times.Once);
            _linkBuilderMock.VerifySet(lb => lb.Success = false, Times.Once);
        }

        [TestMethod]
        public void ProcessInternalShouldSetLinkBuilderSuccessToFalseWhenActionIsNotAuthorized()
        {
            // arrange
            _authorizationProviderMock.Setup(ap => ap.IsAuthorized(It.IsAny<MethodInfo>())).Returns(false);
            var actionMock = new Mock<MethodInfo>(MockBehavior.Strict);
            _linkBuilderMock.SetupGet(lb => lb.Action).Returns(actionMock.Object);
            _linkBuilderMock.SetupSet(lb => lb.Success = It.IsAny<bool>());

            // act
            _handler.ProcessInternal(_registrationMock.Object, _linkBuilderMock.Object, _person);

            // assert
            _authorizationProviderMock.Verify(ap => ap.IsAuthorized(actionMock.Object), Times.Once);
            _linkBuilderMock.VerifyGet(lb => lb.Action, Times.Exactly(2));
            _linkBuilderMock.VerifySet(lb => lb.Success = false, Times.Once);
        }
    }
}