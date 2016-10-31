using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Net.Http;
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
        protected LinkBuilder LinkBuilder;
        protected Person Person;

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

            LinkBuilder = new LinkBuilder(Person);
        }

        protected static IHateoasRegistration<TModel> GetRegistration<TModel, TController>(HttpMethod method = null)
        {
            return GetRegistration(default(Expression<Func<TController, Func<IEnumerable<TModel>>>>), default(Expression<Func<TModel, object>>), method, false);
        }

        protected static IHateoasRegistration<TModel> GetRegistration<TModel, TController>(Expression<Func<TModel, object>> argumentDefinitionExpression, HttpMethod method = null, bool template = false)
        {
            return GetRegistration(default(Expression<Func<TController, Func<IEnumerable<TModel>>>>), argumentDefinitionExpression, method, template);

        }

        protected static IHateoasRegistration<TModel> GetRegistration<TModel, TController>(Expression<Func<TController, Func<IEnumerable<TModel>>>> methodExpression, HttpMethod method = null, bool template = false)
        {
            return GetRegistration(methodExpression, default(Expression<Func<TModel, object>>), method, template);
        }

        private static IHateoasRegistration<TModel> GetRegistration<TModel, TController>(Expression<Func<TController, Func<IEnumerable<TModel>>>> methodExpression, Expression<Func<TModel, object>> argumentDefinitionExpression, HttpMethod method, bool template)
        {
            var registrationMock = new Mock<IHateoasRegistration<TModel>>();

            if (argumentDefinitionExpression != null)
            {
                registrationMock.SetupGet(r => r.ArgumentDefinitions).Returns(new[] { argumentDefinitionExpression });
            }

            var expression = new Mock<IHateoasExpression<TModel>>();
            expression.SetupGet(e => e.Controller).Returns(typeof(TController));
            expression.SetupGet(e => e.Action).Returns(methodExpression);
            expression.SetupGet(e => e.HttpMethod).Returns(method ?? HttpMethod.Get);
            expression.SetupGet(e => e.Template).Returns(template);

            registrationMock.SetupGet(r => r.Expression).Returns(expression.Object);
            return registrationMock.Object;
        }
    }
}