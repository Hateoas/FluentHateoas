using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web.Mvc;
using FluentAssertions;
using FluentHateoas.Handling;
using FluentHateoas.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
// ReSharper disable InvokeAsExtensionMethod

namespace FluentHateoasTest.Helpers
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class LinkHelperTest
    {
        [TestMethod]
        [TestCategory("UnitTest")]
        public void ToLinkListShouldReturnListWithDynamicLinkObjects()
        {
            // arrange
            var source = new Mock<IHateoasLink>().Object;
            var sources = new List<IHateoasLink>
            {
                source
            };

            // act
            var links = LinkHelper.ToLinkList(sources)?.ToList();

            // assert
            links
                .Should().NotBeNull().And
                .HaveCount(sources.Count);
            var link = links.First();

            Assert.AreEqual(source.LinkPath, link.Href);
            Assert.AreEqual(source.Template?.Replace("{", ":").Replace("}", ""), link.Template);
            Assert.AreEqual(source.Relation, link.Rel);
            Assert.AreEqual(source.Method, link.Method);
            Assert.AreEqual(source.Command?.Name, link.Command);
        }
    }
}