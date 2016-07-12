using FluentHateoas.Registration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentHateoasTest
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [TestClass]
    [ExcludeFromCodeCoverage]
    public class HateoasContainerExtensionsTest
    {
        [TestMethod]
        [TestCategory("UnitTest")]
        public void ConfigureMergeTypedOptionsWithDefault()
        {
            // arrange
            var hateoasContainer = HateoasContainerFactory.Create();
            var configuration = new { HrefStyle = HrefStyle.Relative, LinkStyle = LinkStyle.Array, TemplateStyle = TemplateStyle.Rendered };

            // act
            hateoasContainer.Configure(configuration);

            // assert
            Assert.IsNotNull(hateoasContainer.Configuration);
            Assert.AreEqual(HrefStyle.Relative, hateoasContainer.Configuration.HrefStyle);
            Assert.AreEqual(LinkStyle.Array, hateoasContainer.Configuration.LinkStyle);
            Assert.AreEqual(TemplateStyle.Rendered, hateoasContainer.Configuration.TemplateStyle);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public void ConfigureMergeStringOptionsWithDefault()
        {
            // arrange
            var hateoasContainer = HateoasContainerFactory.Create();
            var configuration = new { HrefStyle = "Absolute", LinkStyle = "Array", TemplateStyle = "Rendered" };
            
            // act
            hateoasContainer.Configure(configuration);

            // assert
            Assert.IsNotNull(hateoasContainer.Configuration);
            Assert.AreEqual(HrefStyle.Absolute, hateoasContainer.Configuration.HrefStyle);
            Assert.AreEqual(LinkStyle.Array, hateoasContainer.Configuration.LinkStyle);
            Assert.AreEqual(TemplateStyle.Rendered, hateoasContainer.Configuration.TemplateStyle);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public void ConfigureMergeEmptyOptionsWithDefault()
        {
            // arrange
            var hateoasContainer = HateoasContainerFactory.Create();
            var configuration = new { };

            // act
            hateoasContainer.Configure(configuration);

            // assert
            Assert.IsNotNull(hateoasContainer.Configuration);
            Assert.AreEqual(HrefStyle.Relative, hateoasContainer.Configuration.HrefStyle);
            Assert.AreEqual(LinkStyle.Array, hateoasContainer.Configuration.LinkStyle);
            Assert.AreEqual(TemplateStyle.Rendered, hateoasContainer.Configuration.TemplateStyle);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public void ConfigureFailsWhenUsingNullContainer()
        {
            // arrange

            // act
            try
            {
                HateoasContainerExtensions.Configure(null, new { });

                // assert
                Assert.Fail("Expected ArgumentNullException exception has not been handled.");
            }
            catch (ArgumentNullException exc)
            {
                // assert
                Assert.AreEqual("container", exc.ParamName);
            }
            catch (Exception exc)
            {
                // assert
                Assert.Fail($"Unexpected {exc.GetType().Name} exception: {exc}");
            }
        }
    }
}