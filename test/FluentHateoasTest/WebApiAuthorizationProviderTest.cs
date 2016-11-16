using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.WebPages;
using FluentAssertions;
using FluentHateoas;
using FluentHateoas.Helpers;
using FluentHateoasTest.Assets.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FluentHateoasTest
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class WebApiAuthorizationProviderTest
    {
        private Mock<IHttpContext> _httpContextMock;
        private WebApiAuthorizationProvider _provider;

        [TestInitialize]
        public void TestInitialize()
        {
            _httpContextMock = new Mock<IHttpContext>(MockBehavior.Strict);
            _provider = new WebApiAuthorizationProvider(_httpContextMock.Object);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public void GetRolesShouldReturnEmptyArrayWhenNoRoles()
        {
            // arrange & act & assert
            _provider
                .GetRoles(new AuthorizeAttribute {Roles = ""})
                .Should().HaveCount(0);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public void GetRolesShouldReturnRolesArray()
        {
            // arrange & act & assert
            _provider
                .GetRoles(new AuthorizeAttribute {Roles = "Role 1, Role 2"})
                .Should().HaveCount(2)
                .And.Contain("Role 1", "Role 2")
                .And.BeInAscendingOrder();
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public void GetUsersShouldReturnEmptyArrayWhenNoUsers()
        {
            // arrange & act & assert
            _provider
                .GetUsers(new AuthorizeAttribute { Users = "" })
                .Should().HaveCount(0);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public void GetUsersShouldReturnUsersArray()
        {
            // arrange & act & assert
            _provider
                .GetUsers(new AuthorizeAttribute { Users = "User 1, User 2" })
                .Should().HaveCount(2)
                .And.Contain("User 1", "User 2")
                .And.BeInAscendingOrder();
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public void IsAuthorizedShouldReturnTrueWhenNoAuthorizeAttribute()
        {
            // arrange
            var methodMock = new Mock<MethodInfo>();
            methodMock.SetupGet(m => m.Name).Returns("SomeObscureName");

            // act & assert
            _provider
                .IsAuthorized(methodMock.Object)
                .Should().BeTrue();
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public void IsAuthorizedShouldReturnFalseWhenAuthorizeAttributeAndNotLoggedIn()
        {
            // arrange
            var method = typeof(TestClass).GetMethod(nameof(TestClass.DoSomething));

            var userMock = new Mock<IPrincipal>(MockBehavior.Strict);
            userMock.SetupGet(u => u.Identity).Returns(default(IIdentity));

            _httpContextMock.SetupGet(c => c.User).Returns(userMock.Object);

            // act & assert
            _provider
                .IsAuthorized(method)
                .Should().BeFalse();
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public void IsAuthorizedShouldReturnTrueWhenAuthorizeAttributeAndLoggedIn()
        {
            // arrange
            var method = typeof(TestClass).GetMethod(nameof(TestClass.DoSomething));

            var identityMock = new Mock<IIdentity>(MockBehavior.Strict);
            identityMock.SetupGet(i => i.IsAuthenticated).Returns(true);

            var userMock = new Mock<IPrincipal>(MockBehavior.Strict);
            userMock.SetupGet(u => u.Identity).Returns(identityMock.Object);

            _httpContextMock.SetupGet(c => c.User).Returns(userMock.Object);

            // act & assert
            _provider
                .IsAuthorized(method)
                .Should().BeTrue();
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public void IsAuthorizedShouldReturnFalseWhenAuthorizeAttributeAndLoggedInButNotAuthorizedUser()
        {
            // arrange
            var method = typeof(TestClass).GetMethod(nameof(TestClass.DoSomethingAuthorizedByUser));

            var identityMock = new Mock<IIdentity>(MockBehavior.Strict);
            identityMock.SetupGet(i => i.IsAuthenticated).Returns(true);
            identityMock.SetupGet(i => i.Name).Returns("Unauthorized User");

            var userMock = new Mock<IPrincipal>(MockBehavior.Strict);
            userMock.SetupGet(u => u.Identity).Returns(identityMock.Object);

            _httpContextMock.SetupGet(c => c.User).Returns(userMock.Object);

            // act & assert
            _provider
                .IsAuthorized(method)
                .Should().BeFalse();
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public void IsAuthorizedShouldReturnTrueWhenAuthorizeAttributeAndLoggedInAndAuthorizedUser()
        {
            // arrange
            var method = typeof(TestClass).GetMethod(nameof(TestClass.DoSomethingAuthorizedByUser));

            var identityMock = new Mock<IIdentity>(MockBehavior.Strict);
            identityMock.SetupGet(i => i.IsAuthenticated).Returns(true);
            identityMock.SetupGet(i => i.Name).Returns("User 1");

            var userMock = new Mock<IPrincipal>(MockBehavior.Strict);
            userMock.SetupGet(u => u.Identity).Returns(identityMock.Object);

            _httpContextMock.SetupGet(c => c.User).Returns(userMock.Object);

            // act & assert
            _provider
                .IsAuthorized(method)
                .Should().BeTrue();
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public void IsAuthorizedShouldReturnFalseWhenAuthorizeAttributeAndLoggedInButNotAuthorizedRole()
        {
            // arrange
            var method = typeof(TestClass).GetMethod(nameof(TestClass.DoSomethingAuthorizedByRole));

            var identityMock = new Mock<IIdentity>(MockBehavior.Strict);
            identityMock.SetupGet(i => i.IsAuthenticated).Returns(true);

            var userMock = new Mock<IPrincipal>(MockBehavior.Strict);
            userMock.SetupGet(u => u.Identity).Returns(identityMock.Object);
            userMock.Setup(u => u.IsInRole(It.IsAny<string>())).Returns(false);

            _httpContextMock.SetupGet(c => c.User).Returns(userMock.Object);

            // act & assert
            _provider
                .IsAuthorized(method)
                .Should().BeFalse();
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public void IsAuthorizedShouldReturnTrueWhenAuthorizeAttributeAndLoggedInAndAuthorizedRole()
        {
            // arrange
            var method = typeof(TestClass).GetMethod(nameof(TestClass.DoSomethingAuthorizedByRole));

            var identityMock = new Mock<IIdentity>(MockBehavior.Strict);
            identityMock.SetupGet(i => i.IsAuthenticated).Returns(true);

            var userMock = new Mock<IPrincipal>(MockBehavior.Strict);
            userMock.SetupGet(u => u.Identity).Returns(identityMock.Object);
            userMock.Setup(u => u.IsInRole(It.IsAny<string>())).Returns(true);

            _httpContextMock.SetupGet(c => c.User).Returns(userMock.Object);

            // act & assert
            _provider
                .IsAuthorized(method)
                .Should().BeTrue();
        }

        private class TestClass
        {
            [Authorize]
            public void DoSomething()
            {
                
            }

            [Authorize(Users="User 1")]
            public void DoSomethingAuthorizedByUser()
            {

            }

            [Authorize(Roles = "Role 1")]
            public void DoSomethingAuthorizedByRole()
            {

            }
        }
    }
}
