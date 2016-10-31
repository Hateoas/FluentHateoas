using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using FluentHateoas.Builder.Handlers;
using FluentHateoas.Handling;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FluentHateoasTest.Handling
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class RegistrationLinkHandlerBaseTest
    {
        [TestMethod]
        public void SetSuccessorShouldSucceedIfSuccessorIsValid()
        {
            // arrange
            var handlerMock = new Mock<RegistrationLinkHandlerBase> { CallBase = true };
            var successorMock = new Mock<IRegistrationLinkHandler>(MockBehavior.Strict);

            // act
            handlerMock.Object.SetSuccessor(successorMock.Object);

            // assert (nothing to assert yet, simply should not fail)
        }

        [TestMethod]
        public void SetSuccessorShouldFailIfSuccessorIsNull()
        {
            // arrange
            var handlerMock = new Mock<RegistrationLinkHandlerBase> { CallBase = true };

            // act
            try
            {
                handlerMock.Object.SetSuccessor(null);
            }
            catch (ArgumentNullException) // assert
            {
                return;
            }

            // assert
            Assert.Fail("Expected ArgumentNullException exception has not been thrown.");
        }

        [TestMethod]
        public void ProcessShouldProcessHandlerAndSuccessors()
        {
            // arrange
            Expression<Func<RegistrationLinkHandlerBase, bool>> canProcessExp = h => h.CanProcess<object>(null, null);
            Expression<Action<RegistrationLinkHandlerBase>> processInternalExp = h => h.ProcessInternal<object>(null, null, null);

            var handlerMock1 = new Mock<RegistrationLinkHandlerBase> { CallBase = true };
            handlerMock1.Setup(canProcessExp).Returns(false);
            handlerMock1.Setup(processInternalExp);
            var handlerMock2 = new Mock<RegistrationLinkHandlerBase> { CallBase = true };
            handlerMock2.Setup(canProcessExp).Returns(true);
            handlerMock2.Setup(processInternalExp);
            var handlerMock3 = new Mock<RegistrationLinkHandlerBase> { CallBase = true };
            handlerMock3.Setup(canProcessExp).Returns(false);
            handlerMock3.Setup(processInternalExp);

            // handlerMock1 => handlerMock2 => handlerMock3
            handlerMock2.Object.SetSuccessor(handlerMock3.Object);
            handlerMock1.Object.SetSuccessor(handlerMock2.Object);

            // act
            handlerMock1.Object.Process<object>(null, null, null);

            // assert
            handlerMock1.Verify(canProcessExp, Times.Once);
            handlerMock2.Verify(canProcessExp, Times.Once);
            handlerMock3.Verify(canProcessExp, Times.Once);

            handlerMock1.Verify(processInternalExp, Times.Never);
            handlerMock2.Verify(processInternalExp, Times.Once);
            handlerMock3.Verify(processInternalExp, Times.Never);
        }

    }
}