using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using FluentHateoas.Handling;
using FluentHateoas.Interfaces;
using FluentHateoas.Registration;
using FluentHateoasTest.Assets.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FluentHateoasTest.Registration
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class HttpConfigurationExtensionsTest
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
        public void UpdateConfigurationShouldAddConfigurationWhenItDoesNotExist()
        {
            // arrange

            // act
            var configurationMock = new Mock<IHateoasConfiguration>();
            var expectedConfiguration = configurationMock.Object;
            _httpConfigurationMock.Object.UpdateConfiguration(expectedConfiguration);

            // assert
            object actualConfiguration;
            Assert.IsTrue(_httpConfigurationMock.Object.Properties.TryGetValue(typeof(IHateoasConfiguration), out actualConfiguration));
            Assert.AreEqual(expectedConfiguration, actualConfiguration);
        }

        [TestMethod]
        public void UpdateConfigurationShouldOverwriteConfigurationWhenItDoesExist()
        {
            // arrange
            var existingConfiguration = new Mock<IHateoasConfiguration>().Object;
            _httpConfigurationMock.Object.UpdateConfiguration(existingConfiguration);

            // act
            var expectedConfigurationMock = new Mock<IHateoasConfiguration>();
            var expectedConfiguration = expectedConfigurationMock.Object;
            _httpConfigurationMock.Object.UpdateConfiguration(expectedConfiguration);

            // assert
            object actualConfiguration;
            Assert.IsTrue(_httpConfigurationMock.Object.Properties.TryGetValue(typeof(IHateoasConfiguration), out actualConfiguration));
            Assert.AreEqual(expectedConfiguration, actualConfiguration);
        }

        [TestMethod]
        public void AddRegistrationShouldAddRegistration()
        {
            // arrange
            var registrationMock = new Mock<IHateoasRegistration>();
            registrationMock.SetupGet(r=>r.Model).Returns(typeof(IHateoasRegistration));
            var expected = registrationMock.Object;

            // act
            _httpConfigurationMock.Object.AddRegistration(expected);

            // assert
            var registrations = _httpConfigurationMock.Object.GetRegistrationsFor(typeof(IHateoasRegistration));
            Assert.IsNotNull(registrations);
            Assert.AreEqual(1, registrations.Count);
            Assert.AreEqual(expected, registrations[0]);
        }

        [TestMethod]
        public void UpdateRegistrationShouldAddRegistrationWhenItDoesNotExist()
        {
            // arrange
            var registrationMock = new Mock<IHateoasRegistration>();
            registrationMock.SetupGet(r => r.Model).Returns(typeof(IHateoasRegistration));
            var expected = registrationMock.Object;

            // act
            _httpConfigurationMock.Object.UpdateRegistration(expected);

            // assert
            var registrations = _httpConfigurationMock.Object.GetRegistrationsFor(typeof(IHateoasRegistration));
            Assert.IsNotNull(registrations);
            Assert.AreEqual(1, registrations.Count);
            Assert.AreEqual(expected, registrations[0]);
        }

        [TestMethod]
        public void UpdateRegistrationShouldUpdateExpressionWhenItDoesExist()
        {
            // arrange
            var existingMock = new Mock<IHateoasRegistration>();
            existingMock.SetupGet(r => r.Model).Returns(typeof(IHateoasRegistration));
            var existing = existingMock.Object;
            _httpConfigurationMock.Object.UpdateRegistration(existing);

            // act
            var expectedMock = new Mock<IHateoasRegistration>();
            expectedMock.SetupGet(r => r.Model).Returns(typeof(IHateoasRegistration));
            var expected = expectedMock.Object;
            _httpConfigurationMock.Object.UpdateRegistration(expected);

            // assert
            var registrations = _httpConfigurationMock.Object.GetRegistrationsFor(typeof(IHateoasRegistration));
            Assert.IsNotNull(registrations);
            Assert.AreEqual(1, registrations.Count);
            Assert.AreNotEqual(expected, registrations[0]); // registration itself is not overwritten ...
            Assert.AreEqual(expected.Expression, registrations[0].Expression); // ... just the Expression property
        }

        [TestMethod]
        public void GetRegistrationsForOfTModelShouldGetAllRegistrationsForTModel()
        {
            // arrange
            var registrationMock = new Mock<IHateoasRegistration<Person>>();
            registrationMock.SetupGet(r => r.Model).Returns(typeof(Person));
            var existing = registrationMock.Object;
            _httpConfigurationMock.Object.AddRegistration(existing);

            // act
            var registrations = _httpConfigurationMock.Object.GetRegistrationsFor<Person>();

            // assert
            registrations.Should().NotBeNull().And.HaveCount(1);
            registrationMock.VerifyGet(r => r.Model, Times.Once);
        }

        [TestMethod]
        public void GetRegistrationsForOfTModelShouldGetEmptyListWhenNoRegistrationsExistForTModel()
        {
            // arrange

            // act
            var registrations = _httpConfigurationMock.Object.GetRegistrationsFor<Person>();

            // assert
            registrations.Should().NotBeNull().And.HaveCount(0);
        }

        [TestMethod]
        public void GetRegistrationsForTypeShouldGetAllRegistrationsForType()
        {
            // arrange
            var registrationMock = new Mock<IHateoasRegistration>();
            registrationMock.SetupGet(r => r.Model).Returns(typeof(Person));
            var existing = registrationMock.Object;
            _httpConfigurationMock.Object.AddRegistration(existing);

            // act
            var registrations = _httpConfigurationMock.Object.GetRegistrationsFor(typeof(Person));

            // assert
            registrations.Should().NotBeNull().And.HaveCount(1);
            registrationMock.VerifyGet(r => r.Model, Times.Once);
        }

        [TestMethod]
        public void GetRegistrationsForTypeShouldGetEmptyListWhenNoRegistrationsExistForType()
        {
            // arrange

            // act
            var registrations = _httpConfigurationMock.Object.GetRegistrationsFor(typeof(Person));

            // assert
            registrations.Should().NotBeNull().And.HaveCount(0);
        }
    }
}