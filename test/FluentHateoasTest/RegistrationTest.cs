using System;
using System.Collections.Generic;
using FluentAssertions;
using FluentHateoas.Registration;
using FluentHateoasTest.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentHateoasTest
{
    using System.Diagnostics.CodeAnalysis;

    using FluentHateoas.Contracts;

    [TestClass]
    [ExcludeFromCodeCoverage]
    public class RegistrationTest
    {
        private IHateoasContainer _container;

        [TestInitialize]
        public void TestInitialize()
        {
            _container = HateoasContainerFactory.Create();
        }

        [TestMethod]
        public void RegisterShouldRegisterModel()
        {
            _container.Register<TestModel>();

            _container.Registrations.Count.Should().Be(1);
        }

        [TestMethod]
        public void RegisterIEnumerableAsModelShouldThrowArgumentException()
        {
            try
            {
                _container.Register<IEnumerable<TestModel>>();
                Assert.Fail("Expected exception has not been thrown.");
            }
            catch (ArgumentException exc)
            {
                Assert.AreEqual("Cannot register collections; use .AsCollection() instead", exc.Message);
            }
        }
    }
}