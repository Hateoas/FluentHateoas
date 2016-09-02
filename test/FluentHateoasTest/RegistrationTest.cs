using System;
using System.Collections.Generic;
using FluentHateoas.Registration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Http;

namespace FluentHateoasTest
{
    using System.Diagnostics.CodeAnalysis;

    using Moq;

    [TestClass]
    [ExcludeFromCodeCoverage]
    public class RegistrationTest
    {
        [TestMethod]
        public void RegisterShouldRegisterModel()
        {
            // arrange
            var httpConfiguration = new Mock<HttpConfiguration>().Object;
            var container = HateoasContainerFactory.Create(httpConfiguration);

            // act
            container.Register<TestModel>();

            // assert
            Assert.AreEqual(1, httpConfiguration.GetRegistrationsFor(typeof(TestModel)).Count);
        }

        [TestMethod]
        public void RegisterIEnumerableAsModelShouldThrowArgumentException()
        {
            // arrange
            var httpConfiguration = new Mock<HttpConfiguration>().Object;
            var container = HateoasContainerFactory.Create(httpConfiguration);

            try
            {
                // act
                container.Register<IEnumerable<TestModel>>();

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
            var httpConfiguration = new Mock<HttpConfiguration>().Object;
            var container = HateoasContainerFactory.Create(httpConfiguration);

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

        #region Internal test objects
        // ReSharper disable ClassNeverInstantiated.Local

        private class TestModel
        {
        }

        // ReSharper restore ClassNeverInstantiated.Local
        #endregion
    }
}