namespace FluentHateoasTest
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq.Expressions;
    using System.Net.Http;

    using FluentHateoas.Registration;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Model;

    [TestClass]
    [ExcludeFromCodeCoverage]
    public class HateoasExpressionBuilderTest
    {
        [TestMethod]
        public void ConstructorShouldSaveRegistration()
        {
            // arrange
            const string Relation = "relation";
            Expression<Func<TestModel, object>> identityDefinition = m => m.Id;
            var registration = new HateoasRegistration<TestModel>(Relation, identityDefinition);

            // act
            var builder = new HateoasExpressionBuilder<TestModel>(registration);
            var expression = builder.GetExpression();

            // assert
            Assert.IsNotNull(expression);

            Assert.AreEqual(Relation, expression.Relation);
            Assert.AreEqual(identityDefinition, expression.IdentityDefinition);
            Assert.IsFalse(expression.IsCollection);
        }

        [TestMethod]
        public void GetShouldSaveControllerAndSetHttpMethodGet()
        {
            // arrange
            var registration = new HateoasRegistration<TestModel>(null, null);
            var builder = new HateoasExpressionBuilder<TestModel>(registration);

            // act
            builder.Get<TestModelController>();
            var expression = builder.GetExpression();

            // assert
            Assert.IsNotNull(expression);

            Assert.AreEqual(typeof(TestModelController), expression.Controller);
            Assert.AreEqual(HttpMethod.Get, expression.HttpMethod);
            Assert.IsNull(expression.TargetAction);
        }

        [TestMethod]
        public void GetWithCustomActionShouldSaveControllerAndSetHttpMethodGet()
        {
            // arrange
            var registration = new HateoasRegistration<TestModel>(null, null);
            var builder = new HateoasExpressionBuilder<TestModel>(registration);

            // act
            Expression<Func<TestModelController, Func<IEnumerable<TestModel>>>> getAllExpression = c => c.GetAll;
            builder.Get<TestModelController>(getAllExpression);
            var expression = builder.GetExpression();

            // assert
            Assert.IsNotNull(expression);

            Assert.AreEqual(typeof(TestModelController), expression.Controller);
            Assert.AreEqual(HttpMethod.Get, expression.HttpMethod);
            Assert.AreEqual(getAllExpression, expression.TargetAction);
        }

        [TestMethod]
        public void PostShouldSaveControllerAndSetHttpMethodPost()
        {
            // arrange
            var registration = new HateoasRegistration<TestModel>(null, null);
            var builder = new HateoasExpressionBuilder<TestModel>(registration);

            // act
            builder.Post<TestModelController>();
            var expression = builder.GetExpression();

            // assert
            Assert.IsNotNull(expression);

            Assert.AreEqual(typeof(TestModelController), expression.Controller);
            Assert.AreEqual(HttpMethod.Post, expression.HttpMethod);
            Assert.IsNull(expression.TargetAction);
        }
    }
}