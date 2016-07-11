﻿using FluentHateoas.Registration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentHateoasTest
{
    [TestClass]
    public class HateoasContainerTest
    {
        private HateoasContainer _hateoasContainer;

        [TestInitialize]
        public void TestInitialize()
        {
            _hateoasContainer = HateoasContainerFactory.Create();
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public void ConfigureTestMergeWithDefault()
        {
            _hateoasContainer.Configure(new
            {
                HrefStyle = HrefStyle.Relative,
                LinkStyle = LinkStyle.Array,
                TemplateStyle = TemplateStyle.Rendered
            });

            Assert.IsNotNull(_hateoasContainer.Configuration);
            Assert.AreEqual(HrefStyle.Relative, _hateoasContainer.Configuration.HrefStyle);
            Assert.AreEqual(LinkStyle.Array, _hateoasContainer.Configuration.LinkStyle);
            Assert.AreEqual(TemplateStyle.Rendered, _hateoasContainer.Configuration.TemplateStyle);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public void ConfigureTestMergeWithDefaultUsingValidStringOptions()
        {
            _hateoasContainer.Configure(new
            {
                HrefStyle = "Absolute",
                LinkStyle = "Array",
                TemplateStyle = "Rendered"
            });

            Assert.IsNotNull(_hateoasContainer.Configuration);
            Assert.AreEqual(HrefStyle.Absolute, _hateoasContainer.Configuration.HrefStyle);
            Assert.AreEqual(LinkStyle.Array, _hateoasContainer.Configuration.LinkStyle);
            Assert.AreEqual(TemplateStyle.Rendered, _hateoasContainer.Configuration.TemplateStyle);
        }
    }
}