using System.Diagnostics.CodeAnalysis;
using System.Web.Http;
using FluentAssertions;
using FluentHateoas.Interfaces;
using FluentHateoas.Registration;
using FluentHateoasTest.Assets.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FluentHateoasTest
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class HttpConfigurationExtensionsTest
    {
        [TestMethod]
        public void UpdateConfigurationShouldAddConfigurationWhenItDoesNotExist()
        {
            // arrange
            var httpConfigurationMock = new Mock<HttpConfiguration>();
            var httpConfiguration = httpConfigurationMock.Object;

            // act
            var configurationMock = new Mock<IHateoasConfiguration>();
            var expectedConfiguration = configurationMock.Object;
            httpConfiguration.UpdateConfiguration(expectedConfiguration);

            // assert
            object actualConfiguration;
            Assert.IsTrue(httpConfiguration.Properties.TryGetValue(typeof(IHateoasConfiguration), out actualConfiguration));
            Assert.AreEqual(expectedConfiguration, actualConfiguration);
        }

        [TestMethod]
        public void UpdateConfigurationShouldOverwriteConfigurationWhenItDoesExist()
        {
            // arrange
            var httpConfigurationMock = new Mock<HttpConfiguration>();
            var httpConfiguration = httpConfigurationMock.Object;

            var existingConfiguration = new Mock<IHateoasConfiguration>().Object;
            httpConfiguration.UpdateConfiguration(existingConfiguration);

            // act
            var expectedConfigurationMock = new Mock<IHateoasConfiguration>();
            var expectedConfiguration = expectedConfigurationMock.Object;
            httpConfiguration.UpdateConfiguration(expectedConfiguration);

            // assert
            object actualConfiguration;
            Assert.IsTrue(httpConfiguration.Properties.TryGetValue(typeof(IHateoasConfiguration), out actualConfiguration));
            Assert.AreEqual(expectedConfiguration, actualConfiguration);
        }

        [TestMethod]
        public void AddRegistrationShouldAddRegistration()
        {
            // arrange
            var httpConfigurationMock = new Mock<HttpConfiguration>();
            var httpConfiguration = httpConfigurationMock.Object;

            var registrationMock = new Mock<IHateoasRegistration>();
            registrationMock.SetupGet(r=>r.Model).Returns(typeof(IHateoasRegistration));
            var expected = registrationMock.Object;

            // act
            httpConfiguration.AddRegistration(expected);

            // assert
            var registrations = httpConfiguration.GetRegistrationsFor(typeof(IHateoasRegistration));
            Assert.IsNotNull(registrations);
            Assert.AreEqual(1, registrations.Count);
            Assert.AreEqual(expected, registrations[0]);
        }

        [TestMethod]
        public void UpdateRegistrationShouldAddRegistrationWhenItDoesNotExist()
        {
            // arrange
            var httpConfigurationMock = new Mock<HttpConfiguration>();
            var httpConfiguration = httpConfigurationMock.Object;

            var registrationMock = new Mock<IHateoasRegistration>();
            registrationMock.SetupGet(r => r.Model).Returns(typeof(IHateoasRegistration));
            var expected = registrationMock.Object;

            // act
            httpConfiguration.UpdateRegistration(expected);

            // assert
            var registrations = httpConfiguration.GetRegistrationsFor(typeof(IHateoasRegistration));
            Assert.IsNotNull(registrations);
            Assert.AreEqual(1, registrations.Count);
            Assert.AreEqual(expected, registrations[0]);
        }

        [TestMethod]
        public void UpdateRegistrationShouldUpdateExpressionWhenItDoesExist()
        {
            // arrange
            var httpConfigurationMock = new Mock<HttpConfiguration>();
            var httpConfiguration = httpConfigurationMock.Object;

            var existingMock = new Mock<IHateoasRegistration>();
            existingMock.SetupGet(r => r.Model).Returns(typeof(IHateoasRegistration));
            var existing = existingMock.Object;
            httpConfiguration.UpdateRegistration(existing);

            // act
            var expectedMock = new Mock<IHateoasRegistration>();
            expectedMock.SetupGet(r => r.Model).Returns(typeof(IHateoasRegistration));
            var expected = expectedMock.Object;
            httpConfiguration.UpdateRegistration(expected);

            // assert
            var registrations = httpConfiguration.GetRegistrationsFor(typeof(IHateoasRegistration));
            Assert.IsNotNull(registrations);
            Assert.AreEqual(1, registrations.Count);
            Assert.AreNotEqual(expected, registrations[0]); // registration itself is not overwritten ...
            Assert.AreEqual(expected.Expression, registrations[0].Expression); // ... just the Expression property
        }

        [TestMethod]
        public void GetRegistrationsForOfTModelShouldGetAllRegistrationsForTModel()
        {
            // arrange
            var httpConfigurationMock = new Mock<HttpConfiguration>();
            var httpConfiguration = httpConfigurationMock.Object;

            var registrationMock = new Mock<IHateoasRegistration<Person>>();
            registrationMock.SetupGet(r => r.Model).Returns(typeof(Person));
            var existing = registrationMock.Object;
            httpConfiguration.AddRegistration(existing);

            // act
            var registrations = httpConfiguration.GetRegistrationsFor<Person>();

            // assert
            registrations.Should().NotBeNull().And.HaveCount(1);
            registrationMock.VerifyGet(r => r.Model, Times.Once);
        }

        [TestMethod]
        public void GetRegistrationsForOfTModelShouldGetEmptyListWhenNoRegistrationsExistForTModel()
        {
            // arrange
            var httpConfigurationMock = new Mock<HttpConfiguration>();
            var httpConfiguration = httpConfigurationMock.Object;

            // act
            var registrations = httpConfiguration.GetRegistrationsFor<Person>();

            // assert
            registrations.Should().NotBeNull().And.HaveCount(0);
        }

        [TestMethod]
        public void GetRegistrationsForTypeShouldGetAllRegistrationsForType()
        {
            // arrange
            var httpConfigurationMock = new Mock<HttpConfiguration>();
            var httpConfiguration = httpConfigurationMock.Object;

            var registrationMock = new Mock<IHateoasRegistration>();
            registrationMock.SetupGet(r => r.Model).Returns(typeof(Person));
            var existing = registrationMock.Object;
            httpConfiguration.AddRegistration(existing);

            // act
            var registrations = httpConfiguration.GetRegistrationsFor(typeof(Person));

            // assert
            registrations.Should().NotBeNull().And.HaveCount(1);
            registrationMock.VerifyGet(r => r.Model, Times.Once);
        }

        [TestMethod]
        public void GetRegistrationsForTypeShouldGetEmptyListWhenNoRegistrationsExistForType()
        {
            // arrange
            var httpConfigurationMock = new Mock<HttpConfiguration>();
            var httpConfiguration = httpConfigurationMock.Object;

            // act
            var registrations = httpConfiguration.GetRegistrationsFor(typeof(Person));

            // assert
            registrations.Should().NotBeNull().And.HaveCount(0);
        }
    }
}