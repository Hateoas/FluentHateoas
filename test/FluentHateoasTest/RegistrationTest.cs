using System;
using System.Collections.Generic;
using FluentAssertions;
using FluentHateoas.Registration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentHateoasTest
{
    using System.Diagnostics.CodeAnalysis;

    using FluentHateoas.Contracts;

    [TestClass]
    [ExcludeFromCodeCoverage]
    public class RegistrationTest
    {
        private IHateoasContainer _container;

        [TestInitialize]
        public void TestInitialize()
        {
            _container = HateoasContainerFactory.Create();
        }

        [TestMethod]
        public void RegisterShouldRegisterModel()
        {
            // arrange

            // act
            _container.Register<TestModel>();

            // assert
            _container.Registrations.Count.Should().Be(1);
        }

        [TestMethod]
        public void RegisterIEnumerableAsModelShouldThrowArgumentException()
        {
            // arrange

            try
            {
                // act
                _container.Register<IEnumerable<TestModel>>();

                // assert
                Assert.Fail("Expected exception has not been thrown.");
            }
            catch (ArgumentException exc)
            {
                // assert
                Assert.AreEqual("Cannot register collections; use .AsCollection() instead", exc.Message);
            }
        }

        [TestMethod]
        public void ConfigureShouldMergeDefaultAndProvidedParameters()
        {
            // arrange

            // act
            _container.Configure(new
            {
                HrefStyle = HrefStyle.Relative,
                LinkStyle = LinkStyle.Array,
                TemplateStyle = TemplateStyle.Rendered
            });

            // assert
            Assert.AreEqual(HrefStyle.Relative, _container.Configuration.HrefStyle);
            Assert.AreEqual(LinkStyle.Array, _container.Configuration.LinkStyle);
            Assert.AreEqual(TemplateStyle.Rendered, _container.Configuration.TemplateStyle);
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