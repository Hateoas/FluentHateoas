using System.Diagnostics.CodeAnalysis;
using System.Web.Http;
using System.Web.Http.Dependencies;
using FluentAssertions;
using FluentHateoas;
using FluentHateoas.Contracts;
using FluentHateoas.Handling;
using FluentHateoas.Registration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentHateoasTest
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class HateoasTest
    {
        [TestInitialize]
        public void TestInitialize()
        {
            RegistrationClass.ConstructorCalls = 0;
            RegistrationClass.RegistrationCalls = 0;
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public void StartupShouldCallCreateAndCallRegistrationClassOnce()
        {
            // arrange & act & assert
            Hateoas.Startup<RegistrationClass>(
                new HttpConfiguration(),
                default(IAuthorizationProvider),
                default(IDependencyResolver));

            RegistrationClass.ConstructorCalls.Should().Be(1);
            RegistrationClass.RegistrationCalls.Should().Be(1);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public void StartupOverloadShouldCallCreateAndCallRegistrationClassOnce()
        {
            // arrange & act & assert
            Hateoas.Startup<RegistrationClass>(
                new HttpConfigurationWrapper(new HttpConfiguration()),
                default(IAuthorizationProvider),
                default(IDependencyResolver));

            RegistrationClass.ConstructorCalls.Should().Be(1);
            RegistrationClass.RegistrationCalls.Should().Be(1);
        }

        [ExcludeFromCodeCoverage]
        private class RegistrationClass : IHateoasRegistrationProfile
        {
            public static int ConstructorCalls { get; set; }
            public static int RegistrationCalls { get; set; }

            public RegistrationClass()
            {
                ConstructorCalls++;
            }

            public void Register(IHateoasContainer container)
            {
                RegistrationCalls++;
            }
        }
    }
}