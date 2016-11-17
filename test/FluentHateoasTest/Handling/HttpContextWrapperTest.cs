using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Security.Principal;
using System.Web;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentHateoasTest.Handling
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class HttpContextWrapperTest
    {
        [TestMethod]
        public void TheThingShouldJustWrapCurrentHttpContext()
        {
            // arrange
            var context = new HttpContext(
                new HttpRequest("", "http://tempuri.org", ""),
                new HttpResponse(new StringWriter()))
            {
                User = new GenericPrincipal(new GenericIdentity("name"), new string[0])
            };

            // act & assert
            new FluentHateoas.HttpContextWrapper(context)
                .Should().Match((FluentHateoas.HttpContextWrapper c) => context.User.Equals(c.User));
        }
    }
}