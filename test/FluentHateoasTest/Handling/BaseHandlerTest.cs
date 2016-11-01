using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Net.Http;
using FluentHateoas.Builder.Model;
using FluentHateoas.Handling;
using FluentHateoas.Interfaces;
using FluentHateoasTest.Assets.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FluentHateoasTest.Handling
{
    [ExcludeFromCodeCoverage]
    public abstract class BaseHandlerTest<THandler>
    {
        protected THandler Handler;
        protected Person Person;
        protected Mock<ILinkBuilder> LinkBuilderMock;
        protected ILinkBuilder LinkBuilder;

        [TestInitialize]
        public void BaseInitialize()
        {
            Person = new Person
            {
                Id = Guid.Parse("7AEC12CD-FD43-49DD-A2AB-3CDD19A3A5F4"),
                Birthday = new DateTimeOffset(new DateTime(1980, 1, 1)),
                Firstname = "John",
                Lastname = "Doe"
            };

            LinkBuilderMock = new Mock<ILinkBuilder>(MockBehavior.Strict);
            LinkBuilder = LinkBuilderMock.Object;
        }
    }
}