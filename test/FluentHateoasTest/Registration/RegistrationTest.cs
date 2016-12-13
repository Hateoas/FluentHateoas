using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentHateoas.Handling;
using FluentHateoas.Registration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FluentHateoasTest.Registration
{

    [TestClass]
    [ExcludeFromCodeCoverage]
    public class RegistrationTest
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
        public void RegisterShouldRegisterModel()
        {
            // arrange
            var container = HateoasContainerFactory.Create(_httpConfigurationMock.Object);

            // act
            container.Register<TestModel>("test");

            // assert
            Assert.AreEqual(1, _httpConfigurationMock.Object.GetRegistrationsFor(typeof(TestModel)).Count);
        }

        [TestMethod]
        public void RegisterIEnumerableAsModelShouldThrowArgumentException()
        {
            // arrange
            var container = HateoasContainerFactory.Create(_httpConfigurationMock.Object);

            try
            {
                // act
                container.Register<IEnumerable<TestModel>>("test");

                // assert
                Assert.Fail("Expected exception has not been thrown.");
            }
            catch (ArgumentException exc)
            {
                // assert
                Assert.AreEqual("Cannot register collections; use .RegisterCollection<TModel>(\"name\") instead", exc.Message);
            }
        }

        [TestMethod]
        public void ConfigureShouldMergeDefaultAndProvidedParameters()
        {
            // arrange
            var container = HateoasContainerFactory.Create(_httpConfigurationMock.Object);

            // act
            container.Configure(new
            {
                HrefStyle = HrefStyle.Relative,
                LinkStyle = LinkStyle.Array,
                TemplateStyle = TemplateStyle.Rendered
            });

            // assert
            Assert.AreEqual(HrefStyle.Relative, container.Configuration.HrefStyle);
            Assert.AreEqual(LinkStyle.Array, container.Configuration.LinkStyle);
            Assert.AreEqual(TemplateStyle.Rendered, container.Configuration.TemplateStyle);
        }

        [TestMethod]
        public void RegisterShouldRegisterEnumerableOfTModel()
        {
            // arrange
            var container = HateoasContainerFactory.Create(_httpConfigurationMock.Object);

            // act
            container.RegisterCollection<IEnumerable<TestModel>>("test");

            // assert
            Assert.AreEqual(1, _httpConfigurationMock.Object.GetRegistrationsFor(typeof(IEnumerable<TestModel>)).Count);
        }


        #region Internal test objects
        // ReSharper disable ClassNeverInstantiated.Local

        private class TestModel
        {
        }

        // ReSharper restore ClassNeverInstantiated.Local
        #endregion
    }
}