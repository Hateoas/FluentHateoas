using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FluentAssertions;
using FluentHateoas.Handling;
using FluentHateoasTest.Assets.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FluetHateoasIntegrationTest
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ConfigurationProviderGetLinksForFuncProviderTest
    {
        private Mock<IConfigurationProvider> _configurationProviderMock;
        private Mock<ICache<int, MethodInfo>> _cacheMock;
        private ConfigurationProviderGetLinksForFuncProvider _linksForFuncProvider;

        [TestInitialize]
        public void Initialize()
        {
            _configurationProviderMock = new Mock<IConfigurationProvider>(MockBehavior.Strict);
            _cacheMock = new Mock<ICache<int, MethodInfo>>(MockBehavior.Strict);

            var cacheDictionary = new Dictionary<int, MethodInfo>();
            _cacheMock.Setup(c => c.Get()).Returns(() => cacheDictionary.Values.ToList());
            _cacheMock.Setup(c => c.Add(It.IsAny<int>(), It.IsAny<MethodInfo>())).Callback((int key, MethodInfo value) => cacheDictionary.Add(key, value));

            _linksForFuncProvider = new ConfigurationProviderGetLinksForFuncProvider(_cacheMock.Object);
        }

        [TestMethod]
        public void GetLinksForNonGenericShouldCallLinkFactoryCorrectlyForModel()
        {
            // arrange
            var person = new Person();
            Expression<Func<IConfigurationProvider, IEnumerable<IHateoasLink>>> getLinksForExp = cp => cp.GetLinksFor(It.IsAny<Person>());
            _configurationProviderMock.Setup(getLinksForExp).Returns(default(IEnumerable<IHateoasLink>));


            // act (& assert)
            var linksForFunc = _linksForFuncProvider.GetLinksForFunc(_configurationProviderMock.Object, person.GetType(), person);
            linksForFunc.Should().NotBeNull();
            linksForFunc(_configurationProviderMock.Object, person);

            // assert
            _configurationProviderMock.Verify(getLinksForExp, Times.Once);
        }

        [TestMethod]
        public void GetLinksForNonGenericShouldCallLinkFactoryCorrectlyForIenumerableOfModel()
        {
            // arrange
            var persons = (IEnumerable<Person>) new List<Person> {new Person()};

            Expression<Func<IConfigurationProvider, IEnumerable<IHateoasLink>>> getLinksForExp = cp => cp.GetLinksFor<Person>(It.IsAny<List<Person>>());
            _configurationProviderMock.Setup(getLinksForExp).Returns(default(IEnumerable<IHateoasLink>));

            // act (& assert)
            var linksForFunc = _linksForFuncProvider.GetLinksForFunc(_configurationProviderMock.Object, persons.GetType(), persons);
            linksForFunc.Should().NotBeNull();
            linksForFunc(_configurationProviderMock.Object, persons);

            // assert
            _configurationProviderMock.Verify(getLinksForExp, Times.Once);
        }

        [TestMethod]
        public void GetLinksForNonGenericShouldCallLinkFactoryCorrectlyForListOfModel()
        {
            // arrange
            var persons = new List<Person> { new Person() };

            Expression<Func<IConfigurationProvider, IEnumerable<IHateoasLink>>> getLinksForExp = cp => cp.GetLinksFor<Person>(It.IsAny<List<Person>>());
            _configurationProviderMock.Setup(getLinksForExp).Returns(default(IEnumerable<IHateoasLink>));

            // act (& assert)
            var linksForFunc = _linksForFuncProvider.GetLinksForFunc(_configurationProviderMock.Object, persons.GetType(), persons);
            linksForFunc.Should().NotBeNull();
            linksForFunc(_configurationProviderMock.Object, persons);

            // assert
            _configurationProviderMock.Verify(getLinksForExp, Times.Once);
        }

        [TestMethod]
        public void GetLinksForShouldReuseCachedGenericMethods()
        {
            // arrange
            var persons = new List<Person> { new Person() };

            Expression<Func<IConfigurationProvider, IEnumerable<IHateoasLink>>> getLinksForExp = cp => cp.GetLinksFor<Person>(It.IsAny<List<Person>>());
            _configurationProviderMock.Setup(getLinksForExp).Returns(default(IEnumerable<IHateoasLink>));

            _cacheMock.Setup(c => c.Get()).Returns(
                _configurationProviderMock.Object.GetType()
                    .GetMethods()
                    .Where(m => m.IsGenericMethod && m.Name == nameof(IConfigurationProvider.GetLinksFor)).ToList()
            );

            // act (& assert)
            var linksForFunc = _linksForFuncProvider.GetLinksForFunc(_configurationProviderMock.Object, persons.GetType(), persons);
            linksForFunc.Should().NotBeNull();
            linksForFunc(_configurationProviderMock.Object, persons);

            // assert
            _cacheMock.Verify(c => c.Get(), Times.Once);
            _cacheMock.Verify(c => c.Add(It.IsAny<int>(), It.IsAny<MethodInfo>()), Times.Never);
            _configurationProviderMock.Verify(getLinksForExp, Times.Once);
        }
    }
}