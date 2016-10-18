using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http.Dependencies;
using FluentAssertions;
using FluentHateoas.Contracts;
using FluentHateoas.Handling;
using FluentHateoas.Interfaces;
using FluentHateoas.Registration;
using FluentHateoasTest.Assets.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SampleApi.Model;

namespace FluentHateoasTest.EndToEnd
{
    [TestClass]
    public class SingleEntityEndToEndTest
    {
        private IHateoasConfiguration _configuration;

        private Mock<IAuthorizationProvider> _authorizationProvider;
        private Mock<IDependencyResolver> _dependencyResolver;

        protected IConfigurationProvider ConfigurationProvider;
        protected TestContainer Container;
        protected ILinkFactory LinkFactory;

        protected Person Entity;

        [TestInitialize]
        public void Initialize()
        {
            _configuration = new HateoasConfiguration();

            _authorizationProvider = new Mock<IAuthorizationProvider>();
            _dependencyResolver = new Mock<IDependencyResolver>();

            _authorizationProvider.Setup(p => p.IsAuthorized(It.IsAny<MethodInfo>())).Returns(true);

            Container = new TestContainer(_configuration);
            LinkFactory = new LinkFactory(_authorizationProvider.Object, _dependencyResolver.Object);

            Entity = new Person()
            {
                Id = Guid.Parse("ae213c3e-9ce8-489f-a5ff-5422b55bba44"),
                HouseId = Guid.Parse("a7a52c3d-a5ef-47ff-97db-176f4b2609e4"),
            };
        }

        [TestMethod]
        public void GetDefault()
        {
            Container
                .Register<Person>("list")
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
                .Register<Person>("get-parents", p => p.Id)
                .Get<PersonController>(p => p.GetParents);

            var link = GetLink();

            link.Relation.Should().Be("get-parents");
            link.LinkPath.Should().Be("/api/person/ae213c3e-9ce8-489f-a5ff-5422b55bba44/parents");
            link.Template.Should().BeNull();
            link.Command.Should().BeNull();
        }

        [TestMethod]
        public void GetWithMultipleId()
        {
            Container
                .Register<Person>("get-house", p => p.Id, p => p.HouseId)
                .Get<PersonController>();

            var link = GetLink();

            link.Relation.Should().Be("get-house");
            link.LinkPath.Should().Be("/api/person/ae213c3e-9ce8-489f-a5ff-5422b55bba44/house/a7a52c3d-a5ef-47ff-97db-176f4b2609e4");
            link.Template.Should().BeNull();
            link.Command.Should().BeNull();
        }



        public IHateoasLink GetLink()
        {
            var registrations = Container.GetRegistrationsFor<Person>();
            var links = LinkFactory.CreateLinks(registrations, Entity).ToList();

            links.Count().Should().Be(1);

            return links.Single();
        }
    }

    public class TestContainer : IHateoasContainer
    {
        private readonly List<IHateoasRegistration> _registrations;

        public IHateoasConfiguration Configuration { get; }

        public TestContainer(IHateoasConfiguration configuration)
        {
            Configuration = configuration;

            _registrations = new List<IHateoasRegistration>();
        }

        public void Add(IHateoasRegistration registration)
        {
            _registrations.Add(registration);
        }

        public void Update()
        {
            throw new System.NotImplementedException();
        }

        public void Update(IHateoasRegistration registration)
        {
            var oldRegistration = _registrations.SingleOrDefault(p => p.Model == registration.Model
                && p.IsCollection == registration.IsCollection
                && p.Relation == registration.Relation);

            if (oldRegistration != null)
                _registrations.Remove(oldRegistration);

            _registrations.Add(registration);
        }

        public IHateoasRegistration<TModel> GetRegistration<TModel>(string relation)
        {
            var modelType = typeof(TModel);
            return _registrations.SingleOrDefault(p => p.Model == modelType && p.Relation == relation) as IHateoasRegistration<TModel>;
        }

        public List<IHateoasRegistration<TModel>> GetRegistrationsFor<TModel>()
        {
            var modelType = typeof(TModel);
            return _registrations.Where(p => p.Model == modelType).Select(p => p as IHateoasRegistration<TModel>).ToList();
        }
    }
}