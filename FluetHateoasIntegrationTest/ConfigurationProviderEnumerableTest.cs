using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Web.Http.Dependencies;
using FluentAssertions;
using FluentHateoas.Builder.Handlers;
using FluentHateoas.Handling;
using FluentHateoas.Registration;
using FluentHateoasTest.Assets.Controllers;
using FluentHateoasTest.Assets.Model;
using FluentHateoasTest.Assets.Providers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FluetHateoasIntegrationTest
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ConfigurationProviderEnumerableTest
    {
        private Mock<IAuthorizationProvider> _authorizationProvider;
        private Mock<IDependencyResolver> _dependencyResolverMock;
        private Mock<IPersonProvider> _personProvider;

        protected IConfigurationProvider ConfigurationProvider;
        protected HateoasContainer Container;
        protected ILinkFactory LinkFactory;

        protected IEnumerable<Person> Enumerable;

        [TestInitialize]
        public void Initialize()
        {
            _authorizationProvider = new Mock<IAuthorizationProvider>();
            _dependencyResolverMock = new Mock<IDependencyResolver>();
            _personProvider = new Mock<IPersonProvider>();

            _authorizationProvider.Setup(p => p.IsAuthorized(It.IsAny<MethodInfo>())).Returns(true);
            _dependencyResolverMock.Setup(p => p.GetService(It.IsAny<Type>())).Returns(_personProvider.Object);

            var idFromExpressionProcessor = new IdFromExpressionProcessor(_dependencyResolverMock.Object);
            var argumentsDefinitionsProcessor = new ArgumentDefinitionsProcessor();
            var templateArgumentsProcessor = new TemplateArgumentsProcessor();

            var configurationMock = new Mock<IHttpConfiguration>(MockBehavior.Strict);
            var properties = new ConcurrentDictionary<object, object>();
            configurationMock.SetupGet(c => c.Properties).Returns(() => properties);

            Container = HateoasContainerFactory.Create(configurationMock.Object);
            LinkFactory = new LinkFactory(
                _authorizationProvider.Object, 
                idFromExpressionProcessor,
                argumentsDefinitionsProcessor,
                templateArgumentsProcessor);
            var linksForFuncProvider = new ConfigurationProviderGetLinksForFuncProvider(new InMemoryCache<int, MethodInfo>());
            ConfigurationProvider = new ConfigurationProvider(configurationMock.Object, LinkFactory, linksForFuncProvider, new InMemoryCache<Type, Func<ConfigurationProvider, object, IEnumerable<IHateoasLink>>>());

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

            // this link is added to check if it is ignored
            Container.Register<Person>("some-extra-link").Get<PersonController>();
            Container.Register<Car>("some-extra-link").Get<CarController>();
        }

        [TestMethod]
        public void GetDefault()
        {
            Container
                .RegisterCollection<Person>("list")
                .Get<PersonController>();

            var link = GetLink();

            link.Relation.Should().Be("list");
            link.LinkPath.Should().Be("/api/person");
            link.Template.Should().BeNull();
            link.Command.Should().BeNull();
        }

        [TestMethod]
        public void GetDefaultWithIdAsTemplate()
        {
            Container
                .RegisterCollection<Person>("item")
                .Get<CarController>()
                .AsTemplate(p => p.Id);

            var link = GetLink();

            link.Relation.Should().Be("item");
            link.LinkPath.Should().BeNull();
            link.Template.Should().Be("/api/car/{id}");
            link.Command.Should().BeNull();
        }

        [TestMethod]
        public void GetDefaultWithIdAsArgumentAndTemplate()
        {
            Container
                .RegisterCollection<Person>("item", p => p.Id)
                .Get<PersonController>()
                .AsTemplate(p => p.Id);

            var link = GetLink();

            link.Relation.Should().Be("item");
            link.LinkPath.Should().BeNull();
            link.Template.Should().Be("/api/person/{id}");
            link.Command.Should().BeNull();
        }

        [TestMethod]
        public void GetWithMethod()
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
            link.LinkPath.Should().BeNull();
            link.Template.Should().Be("/api/person/{id}/house/{houseId}");
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
        [Ignore]
        public void GetByCondition()
        {
            // todo: implement this for collection
            _personProvider.Setup(p => p.HasNextId(It.IsAny<object>())).Returns(true);

            //Container
            //    .RegisterCollection<Person>("display-template")
            //    .Get<PersonController>()
            //    .When<IPersonProvider>(((provider, persons) => true));

            var link = GetLink();

            link.Relation.Should().Be("display-template");
            link.LinkPath.Should().Be("/api/person");
            link.Template.Should().BeNull();
            link.Command.Should().BeNull();
        }


        public IHateoasLink GetLink()
        {
            var carLink = ConfigurationProvider.GetLinksFor(new Car());
            var personLink = ConfigurationProvider.GetLinksFor(new Person());
            var links = ConfigurationProvider.GetLinksFor(Enumerable).ToList();

            carLink.Count().Should().Be(1);
            personLink.Count().Should().Be(1);
            links.Count().Should().Be(1);

            return links.Single();
        }
    }
}