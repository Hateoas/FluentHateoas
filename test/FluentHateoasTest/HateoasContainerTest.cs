using System.Collections.Concurrent;
using FluentHateoas.Handling;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using HttpConfigurationExtensions = FluentHateoas.Registration.HttpConfigurationExtensions;

namespace FluentHateoasTest
{
    [TestClass]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class HateoasContainerTest
    {
        private Mock<IHttpConfiguration> _httpConfigurationMock;

        [TestInitialize]
        public void Initialize()
        {
            _httpConfigurationMock = new Mock<IHttpConfiguration>();
            var properties = new ConcurrentDictionary<object, object>();
            _httpConfigurationMock.SetupGet(c => c.Properties).Returns(() => properties);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public void AddAddsRegistrationToHttpConfigurationProperties()
        {
            // arrange
            var hateoasContainer = FluentHateoas.Registration.HateoasContainerFactory.Create(_httpConfigurationMock.Object);

            var expectedRegMock = new Mock<FluentHateoas.Interfaces.IHateoasRegistration>();
            expectedRegMock.SetupGet(r => r.Model).Returns(typeof(FluentHateoas.Interfaces.IHateoasRegistration));
            var expectedReg = expectedRegMock.Object;

            // act
            hateoasContainer.Add(expectedReg);

            // assert
            var actualRegs = HttpConfigurationExtensions.GetRegistrationsFor(_httpConfigurationMock.Object, typeof(FluentHateoas.Interfaces.IHateoasRegistration));
            Assert.IsNotNull(actualRegs);
            Assert.AreEqual(1, actualRegs.Count);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public void UpdateUpdatesRegistrationToHttpConfigurationProperties()
        {
            // arrange
            var hateoasContainer = FluentHateoas.Registration.HateoasContainerFactory.Create(_httpConfigurationMock.Object);

            var expectedRegMock = new Mock<FluentHateoas.Interfaces.IHateoasRegistration>();
            expectedRegMock.SetupGet(r => r.Model).Returns(typeof(FluentHateoas.Interfaces.IHateoasRegistration));
            var expectedReg = expectedRegMock.Object;

            // act
            hateoasContainer.Update(expectedReg);

            // assert
            var actualRegs = HttpConfigurationExtensions.GetRegistrationsFor(_httpConfigurationMock.Object, typeof(FluentHateoas.Interfaces.IHateoasRegistration));
            Assert.IsNotNull(actualRegs);
            Assert.AreEqual(1, actualRegs.Count);
        }
    }
}