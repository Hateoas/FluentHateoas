using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentHateoas.Handling;
using FluentHateoas.Interfaces;
using FluentHateoas.Registration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SampleApi.Model;

namespace FluentHateoasTest.Handling
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ConfigurationProviderTest
    {
        private Mock<ILinkFactory> _linkFactoryMock;
        private Mock<IConfigurationProviderGetLinksForFuncProvider> _linksProviderMock;
        private Mock<ICache<Type, Func<ConfigurationProvider, object, IEnumerable<IHateoasLink>>>> _cacheMock;
        private ConfigurationProvider _configurationProvider;
        private Mock<IHttpConfiguration> _httpConfigurationMock;
        private Mock<IHateoasRegistration<Person>> _registration1Mock;
        private Mock<IHateoasRegistration<Person>> _registration2Mock;

        [TestInitialize]
        public void Initialize()
        {
            _registration1Mock = new Mock<IHateoasRegistration<Person>>(MockBehavior.Strict);
            _registration1Mock.SetupGet(r => r.Model).Returns(typeof(Person));
            _registration1Mock.SetupGet(r => r.IsCollection).Returns(false);

            _registration2Mock = new Mock<IHateoasRegistration<Person>>(MockBehavior.Strict);
            _registration2Mock.SetupGet(r => r.Model).Returns(typeof(Person));
            _registration2Mock.SetupGet(r => r.IsCollection).Returns(true);

            _httpConfigurationMock = new Mock<IHttpConfiguration>(MockBehavior.Strict);
            var properties = new ConcurrentDictionary<object, object>();
            _httpConfigurationMock.SetupGet(hc => hc.Properties).Returns(() => properties);
            _httpConfigurationMock.Object.AddRegistration(_registration1Mock.Object);
            _httpConfigurationMock.Object.AddRegistration(_registration2Mock.Object);

            _linkFactoryMock = new Mock<ILinkFactory>(MockBehavior.Strict);

            _linksProviderMock = new Mock<IConfigurationProviderGetLinksForFuncProvider>(MockBehavior.Strict);
            _cacheMock = new Mock<ICache<Type, Func<ConfigurationProvider, object, IEnumerable<IHateoasLink>>>>(MockBehavior.Strict);

            _configurationProvider = new ConfigurationProvider(
                _httpConfigurationMock.Object,
                _linkFactoryMock.Object,
                _linksProviderMock.Object,
                _cacheMock.Object);
        }

        [TestMethod]
        public void GetLinksForShouldReturnLinksForSingleOfTModel()
        {
            // arrange
            var data = new Person();

            _linkFactoryMock.Setup(lf => lf.CreateLinks(It.IsAny<IEnumerable<IHateoasRegistration<Person>>>(), data)).Returns(default(IEnumerable<IHateoasLink>));
            
            // act
            _configurationProvider.GetLinksFor(data);

            // assert
            _httpConfigurationMock.VerifyGet(c => c.Properties, Times.AtLeastOnce);
            _linkFactoryMock.Verify(
                lf => lf.CreateLinks(
                    It.Is<IEnumerable<IHateoasRegistration<Person>>>(r => r.Count() == 1),
                    data),
                Times.Once);
        }

        [TestMethod]
        public void GetLinksForShouldReturnLinksForEnumerableOfTModel()
        {
            // arrange
            var data = new List<Person> { new Person() };

            _linkFactoryMock.Setup(lf => lf.CreateLinks(
                    It.IsAny<IEnumerable<IHateoasRegistration<Person>>>(),
                    data)
            ).Returns(default(IEnumerable<IHateoasLink>));

            // act
            _configurationProvider.GetLinksFor<Person>(data);

            // assert
            _httpConfigurationMock.VerifyGet(c => c.Properties, Times.AtLeastOnce);
            _linkFactoryMock.Verify(
                lf => lf.CreateLinks(
                    It.Is<IEnumerable<IHateoasRegistration<Person>>>(r => r.Count() == 1),
                    data),
                Times.Once);
        }

        [TestMethod]
        public void GetLinksForShouldAddCompiledFunctionToCompiledFunctionCache()
        {
            // arrange
            var data = new Person();
            Func<IConfigurationProvider, object, IEnumerable<IHateoasLink>> getLinksForFunc = (provider, o) => provider.GetLinksFor((Person)o);

            // ReSharper disable ImplicitlyCapturedClosure
            _cacheMock
                .Setup(c => c.Get(data.GetType()))
                .Returns(default(Func<ConfigurationProvider, object, IEnumerable<IHateoasLink>>));
            _cacheMock.Setup(c => c.Add(data.GetType(), getLinksForFunc));

            _linkFactoryMock
                .Setup(lf => lf.CreateLinks(It.IsAny<IEnumerable<IHateoasRegistration<Person>>>(), data))
                .Returns(default(IEnumerable<IHateoasLink>));

            _linksProviderMock
                .Setup(lf => lf.GetLinksForFunc(It.IsAny<IConfigurationProvider>(), data.GetType(), data))
                .Returns(getLinksForFunc);
            // ReSharper restore ImplicitlyCapturedClosure

            // act
            _configurationProvider.GetLinksFor(data.GetType(), data);

            // assert
            _httpConfigurationMock.VerifyGet(c => c.Properties, Times.AtLeastOnce);
            _linkFactoryMock.Verify(
                lf => lf.CreateLinks(
                    It.Is<IEnumerable<IHateoasRegistration<Person>>>(r => r.Count() == 1),
                    data),
                Times.Once);
        }
        [TestMethod]
        public void GetLinksForShouldReuseCompiledFunctionCache()
        {
            // arrange
            var data = new Person();

            _cacheMock.Setup(c => c.Get(data.GetType()))
                .Returns((provider, o) => provider.GetLinksFor((Person)o));

            _linkFactoryMock
                .Setup(lf => lf.CreateLinks(It.IsAny<IEnumerable<IHateoasRegistration<Person>>>(), data))
                .Returns(default(IEnumerable<IHateoasLink>));

            // act
            _configurationProvider.GetLinksFor(data.GetType(), data);

            // assert
            _httpConfigurationMock.VerifyGet(c => c.Properties, Times.AtLeastOnce);
            _linkFactoryMock.Verify(
                lf => lf.CreateLinks(
                    It.Is<IEnumerable<IHateoasRegistration<Person>>>(r => r.Count() == 1),
                    data),
                Times.Once);
        }
    }
}