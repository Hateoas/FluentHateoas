using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Dependencies;
using FluentAssertions;
using FluentHateoas.Handling;
using FluentHateoas.Interfaces;
using FluentHateoas.Registration;
using FluentHateoasTest.Assets.Controllers;
using FluentHateoasTest.Assets.Model;
using FluentHateoasTest.Assets.Providers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FluentHateoasTest.EndToEnd
{
    [TestClass]
    public class EnumerableTest
    {
        private IHateoasConfiguration _configuration;

        private Mock<IAuthorizationProvider> _authorizationProvider;
        private Mock<IDependencyResolver> _dependencyResolver;
        private Mock<IPersonProvider> _personProvider;

        protected IConfigurationProvider ConfigurationProvider;
        protected HateoasContainer Container;
        protected ILinkFactory LinkFactory;

        protected IEnumerable<Person> Enumerable;

        [TestInitialize]
        public void Initialize()
        {
            _configuration = new HateoasConfiguration();

            _authorizationProvider = new Mock<IAuthorizationProvider>();
            _dependencyResolver = new Mock<IDependencyResolver>();
            _personProvider = new Mock<IPersonProvider>();

            _authorizationProvider.Setup(p => p.IsAuthorized(It.IsAny<MethodInfo>())).Returns(true);
            _dependencyResolver.Setup(p => p.GetService(It.IsAny<Type>())).Returns(_personProvider.Object);

            var configuration = new HttpConfiguration();
            Container = HateoasContainerFactory.Create(configuration);
            LinkFactory = new LinkFactory(_authorizationProvider.Object, _dependencyResolver.Object);
            ConfigurationProvider = new ConfigurationProvider(configuration, LinkFactory);

            Enumerable = new List<Person>
            {
                new Person()
                {
                    Id = Guid.Parse("ae213c3e-9ce8-489f-a5ff-5422b55bba44"),
                    HouseId = Guid.Parse("a7a52c3d-a5ef-47ff-97db-176f4b2609e4"),
                },
                new Person()
                {
                    Id = Guid.Parse("6153d85e-8233-4d9f-b583-a78d2ba8d3d1"),
                    HouseId = Guid.Parse("9042e54f-dee6-45c9-8d9e-048026f5d5fa"),
                },
            };
        }

        [TestMethod]
        public void GetDefault()
        {
            Container
                .Register<IEnumerable<Person>>("list")
                .Get<PersonController>();

            var link = GetLink();

            link.Relation.Should().Be("list");
            link.LinkPath.Should().Be("/api/person");
            link.Template.Should().BeNull();
            link.Command.Should().BeNull();
        }

        [TestMethod]
        public void GetWithMethod()
        {
            Container
                .Register<IEnumerable<Person>>("get-parents")
                .Get<PersonController>(p => p.GetParents);

            var link = GetLink();

            link.Relation.Should().Be("get-parents");
            link.LinkPath.Should().Be("/api/person/{id}/parents");
            link.Template.Should().BeNull();
            link.Command.Should().BeNull();
        }

        [TestMethod]
        public void GetWithAsCollectionMethod()
        {
            Container
                .RegisterCollection<Person>("get-parents")
                .Get<PersonController>(p => p.GetParents);

            var link = GetLink();

            link.Relation.Should().Be("get-parents");
            link.LinkPath.Should().Be("/api/person/{id}/parents");
            link.Template.Should().BeNull();
            link.Command.Should().BeNull();
        }

        [TestMethod]
        public void GetWithMultipleId()
        {
            Container
                .RegisterCollection<Person>("get-house")
                .Get<PersonController>()
                .AsTemplate(p => p.Id, p => p.HouseId);

            var link = GetLink();

            link.Relation.Should().Be("get-house");
            link.LinkPath.Should().Be("/api/person/{id}/house/{houseId}");
            link.Template.Should().BeNull();
            link.Command.Should().BeNull();
        }

        [TestMethod]
        [Ignore]
        public void GetByIdWithDefinedAction()
        {
            // todo: is this even possible with overloads??
            //Container
            //    .Register<Person>("get-by-id-with-action", p => p.Id)
            //    .Get<PersonController>(p => p.Get);

            var link = GetLink();

            link.Relation.Should().Be("get-by-id-with-action");
            link.LinkPath.Should().Be("/api/person/ae213c3e-9ce8-489f-a5ff-5422b55bba44/house/a7a52c3d-a5ef-47ff-97db-176f4b2609e4");
            link.Template.Should().BeNull();
            link.Command.Should().BeNull();
        }

        [TestMethod]
        public void GetByCondition()
        {
            _personProvider.Setup(p => p.HasNextId(It.IsAny<object>())).Returns(true);

            Container
                .Register<IEnumerable<Person>>("display-template")
                .Get<PersonController>()
                .When<IPersonProvider>(((provider, persons) => persons.Any() && provider.HasNextId(persons.First())));

            var link = GetLink();

            link.Relation.Should().Be("display-template");
            link.LinkPath.Should().Be("/api/person");
            link.Template.Should().BeNull();
            link.Command.Should().BeNull();
        }


        public IHateoasLink GetLink()
        {
            var links = ConfigurationProvider.GetLinksFor(Enumerable).ToList();

            links.Count().Should().Be(1);

            return links.Single();
        }
    }
}