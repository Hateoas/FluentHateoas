using System;
using FluentHateoas.Builder.Handlers;
using FluentHateoas.Handling;
using FluentHateoas.Registration;
using FluentHateoasTest.Assets;
using FluentHateoasTest.Assets.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentHateoasTest.Handling
{
    public abstract class BaseHandlerTest<THandler> where THandler: new()
    {
        private HateoasConfiguration _configuration;

        protected THandler Handler;
        protected TestContainer Container;
        protected LinkBuilder LinkBuilder;
        protected Person Person;

        [TestInitialize]
        public void Initialize()
        {
            Person = new Person
            {
                Id = Guid.Parse("7AEC12CD-FD43-49DD-A2AB-3CDD19A3A5F4"),
                Birthday = new DateTimeOffset(new DateTime(1980, 1, 1)),
                Firstname = "John",
                Lastname = "Doe"
            };

            _configuration = new HateoasConfiguration();
            Container = new TestContainer(_configuration);

            Handler = new THandler();
            LinkBuilder = new LinkBuilder();
        }

    }
}