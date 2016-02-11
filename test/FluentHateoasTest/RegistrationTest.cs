using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using FluentHateoas.Registration;
using FluentHateoasTest.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentHateoasTest
{
    [TestClass]
    public class RegistrationTest
    {
        private HateoasContainer _container;

        [TestInitialize]
        public void Initialize()
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
        public void RegisterIEnumerableShouldThrowArgumentException()
        {
            Action registration = () => _container.Register<IEnumerable<TestModel>>();

            registration.ShouldThrow<ArgumentException>();
        }
    }
}