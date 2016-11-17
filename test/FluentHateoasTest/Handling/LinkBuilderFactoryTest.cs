using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using FluentHateoas.Handling;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentHateoasTest.Handling
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class LinkBuilderFactoryTest
    {
        [TestMethod]
        public void GetLinkBuilderShouldReturnLinkBuilder()
        {
            // arrange & act & assert
            new LinkBuilderFactory()
                .GetLinkBuilder(null)
                .Should().NotBeNull()
                .And.BeOfType<LinkBuilder>();
        }

        [TestMethod]
        public void GetLinkBuilderShouldReturnNewLinkBuilderForEachCall()
        {
            // arrange
            var factory = new LinkBuilderFactory();

            // act & assert
            var linkBuilder1 = factory
                .GetLinkBuilder(null)
                .Should().NotBeNull()
                .And.BeOfType<LinkBuilder>();

            factory
                .GetLinkBuilder(null)
                .Should().NotBeNull()
                .And.BeOfType<LinkBuilder>()
                .And.NotBe(linkBuilder1);
        }
    }
}