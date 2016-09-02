using HttpConfigurationExtensions = FluentHateoas.Registration.HttpConfigurationExtensions;

namespace FluentHateoasTest
{
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClass]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class HateoasContainerTest
    {
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategory("UnitTest")]
        public void AddAddsRegistrationToHttpConfigurationProperties()
        {
            // arrange
            var httpConfigurationMock = new Moq.Mock<System.Web.Http.HttpConfiguration>();
            var httpConfiguration = httpConfigurationMock.Object;
            var hateoasContainer = FluentHateoas.Registration.HateoasContainerFactory.Create(httpConfiguration);

            var expectedRegMock = new Moq.Mock<FluentHateoas.Interfaces.IHateoasRegistration>();
            expectedRegMock.SetupGet(r => r.Model).Returns(typeof(FluentHateoas.Interfaces.IHateoasRegistration));
            var expectedReg = expectedRegMock.Object;

            // act
            hateoasContainer.Add(expectedReg);

            // assert
            var actualRegs = HttpConfigurationExtensions.GetRegistrationsFor(httpConfiguration, typeof(FluentHateoas.Interfaces.IHateoasRegistration));
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(actualRegs);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(1, actualRegs.Count);
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategory("UnitTest")]
        public void UpdateUpdatesRegistrationToHttpConfigurationProperties()
        {
            // arrange
            var httpConfigurationMock = new Moq.Mock<System.Web.Http.HttpConfiguration>();
            var httpConfiguration = httpConfigurationMock.Object;
            var hateoasContainer = FluentHateoas.Registration.HateoasContainerFactory.Create(httpConfiguration);

            var expectedRegMock = new Moq.Mock<FluentHateoas.Interfaces.IHateoasRegistration>();
            expectedRegMock.SetupGet(r => r.Model).Returns(typeof(FluentHateoas.Interfaces.IHateoasRegistration));
            var expectedReg = expectedRegMock.Object;

            // act
            hateoasContainer.Update(expectedReg);

            // assert
            var actualRegs = HttpConfigurationExtensions.GetRegistrationsFor(httpConfiguration, typeof(FluentHateoas.Interfaces.IHateoasRegistration));
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(actualRegs);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(1, actualRegs.Count);
        }
    }
}