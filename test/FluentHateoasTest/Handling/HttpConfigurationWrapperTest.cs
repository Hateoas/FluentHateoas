using System.Diagnostics.CodeAnalysis;
using System.Web.Http;
using FluentAssertions;
using FluentHateoas.Handling;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentHateoasTest.Handling
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class HttpConfigurationWrapperTest
    {
        [TestMethod]
        public void TheThingShouldJustWrapHttpConfiguration()
        {
            // arrange
            var configuration = new HttpConfiguration();

            // act & assert
            new HttpConfigurationWrapper(configuration)
                .Should().Match((HttpConfigurationWrapper c) =>
                    configuration.MessageHandlers.Equals(c.MessageHandlers) &&
                    configuration.Properties.Equals(c.Properties));
        }
    }
}