using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using FluentHateoas.Handling;
using FluentHateoasTest.Assets.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FluetHateoasIntegrationTest
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ConfigurationProviderExtensionsTest
    {
        [TestMethod]
        public void GetLinksForNonGenericShouldCallLinkFactoryCorrectlyForModel()
        {
            // arrange
            var person = new Person();

            var configurationProviderMock = new Mock<IConfigurationProvider>(MockBehavior.Strict);
            Expression<Func<IConfigurationProvider, IEnumerable<IHateoasLink>>> getLinksForExp = cp => cp.GetLinksFor(It.IsAny<Person>());
            configurationProviderMock.Setup(getLinksForExp).Returns(default(IEnumerable<IHateoasLink>));
            var configurationProvider = configurationProviderMock.Object;

            // act & assert
            var linksForFunc = configurationProvider.GetLinksForFunc(person.GetType(), person);

            Assert.IsNotNull(linksForFunc);

            linksForFunc(configurationProvider, person);
            configurationProviderMock.Verify(getLinksForExp, Times.Once);
        }

        [TestMethod]
        public void GetLinksForNonGenericShouldCallLinkFactoryCorrectlyForIenumerableOfModel()
        {
            // arrange
            var persons = (IEnumerable<Person>) new List<Person> {new Person()};

            var configurationProviderMock = new Mock<IConfigurationProvider>(MockBehavior.Strict);
            Expression<Func<IConfigurationProvider, IEnumerable<IHateoasLink>>> getLinksForExp = cp => cp.GetLinksFor<Person>(It.IsAny<List<Person>>());
            configurationProviderMock.Setup(getLinksForExp).Returns(default(IEnumerable<IHateoasLink>));
            var configurationProvider = configurationProviderMock.Object;

            // act & assert
            var linksForFunc = configurationProvider.GetLinksForFunc(persons.GetType(), persons);

            Assert.IsNotNull(linksForFunc);

            linksForFunc(configurationProvider, persons);
            configurationProviderMock.Verify(getLinksForExp, Times.Once);
        }

        [TestMethod]
        public void GetLinksForNonGenericShouldCallLinkFactoryCorrectlyForListOfModel()
        {
            // arrange
            var persons = new List<Person> { new Person() };

            var configurationProviderMock = new Mock<IConfigurationProvider>(MockBehavior.Strict);
            Expression<Func<IConfigurationProvider, IEnumerable<IHateoasLink>>> getLinksForExp = cp => cp.GetLinksFor<Person>(It.IsAny<List<Person>>());
            configurationProviderMock.Setup(getLinksForExp).Returns(default(IEnumerable<IHateoasLink>));
            var configurationProvider = configurationProviderMock.Object;

            // act & assert
            var linksForFunc = configurationProvider.GetLinksForFunc(persons.GetType(), persons);

            Assert.IsNotNull(linksForFunc);

            linksForFunc(configurationProvider, persons);
            configurationProviderMock.Verify(getLinksForExp, Times.Once);
        }

    }
}