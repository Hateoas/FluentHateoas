using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Dependencies;
using FluentHateoas.Handling;
using FluentHateoas.Registration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FluentHateoasTest.Handling
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class RegistrationLinkHandlerExtensionsTest
    {
        [TestMethod]
        public void CreateChainShouldSetSuccessorsAndReturnFirstHandler()
        {
            // arrange
            Expression<Action<IRegistrationLinkHandler>> expression =
                h => h.SetSuccessor(It.IsAny<IRegistrationLinkHandler>());

            var handlerMock1 = new Mock<IRegistrationLinkHandler>();
            handlerMock1.Setup(expression);
            var handlerMock2 = new Mock<IRegistrationLinkHandler>();
            handlerMock2.Setup(expression);
            var handlerMock3 = new Mock<IRegistrationLinkHandler>();
            handlerMock3.Setup(expression);

            var handlers = new List<IRegistrationLinkHandler>
            {
                handlerMock1.Object,
                handlerMock2.Object,
                handlerMock3.Object
            };

            // act
            var handlerChain = handlers.CreateChain();

            // assert
            handlerMock1.Verify(expression, Times.Once);
            handlerMock2.Verify(expression, Times.Once);
            handlerMock3.Verify(expression, Times.Never);
            Assert.AreEqual(handlerMock1.Object, handlerChain);
        }
    }
}