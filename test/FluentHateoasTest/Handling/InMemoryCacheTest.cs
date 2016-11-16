using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using FluentHateoas.Handling;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentHateoasTest.Handling
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class InMemoryCacheTest
    {
        private InMemoryCache<string, string> _cache;

        [TestInitialize]
        public void Initialize()
        {
            _cache = new InMemoryCache<string,string>();
        }

        [TestMethod]
        public void AddAddsItemToCache()
        {
            // arrange
            const string key = "key";
            const string value = "value";

            // act 
            _cache.Add(key, value);

            // assert
            _cache.Get().Should().HaveCount(1);
            _cache.Get(key).Should().BeEquivalentTo(value);
        }

        [TestMethod]
        public void GetByKeyShouldGetExistingItemFromCache()
        {
            // arrange
            const string key = "key";
            const string value = "value";

            _cache.Add(key, value);

            // act & assert
            _cache.Get(key).Should().BeEquivalentTo(value);
        }

        [TestMethod]
        public void GetByKeyShouldGetReturnNullWhenNoExistingItemInCache()
        {
            // arrange
            const string key = "key";
            const string value = "value";

            _cache.Add(key, value);

            // act & assert
            _cache.Get("non-existing").Should().BeNull();
        }
    }
}