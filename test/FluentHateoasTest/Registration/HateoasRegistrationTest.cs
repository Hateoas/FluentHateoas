// ReSharper disable InconsistentNaming

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using FluentAssertions;
using FluentHateoas.Interfaces;
using FluentHateoas.Registration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FluentHateoasTest.Registration
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class HateoasRegistrationTest
    {
        [TestMethod]
        public void ConstructorWithoutParametersShouldSetCorrectProperties()
        {
            // arrange
            const string relation = "self";

            // act
            var registration = new HateoasRegistration<TestModel>(null);

            // assert
            Assert.AreEqual(relation, registration.Relation);
            Assert.IsNull(registration.ArgumentDefinitions);
            Assert.IsFalse(registration.IsCollection);
        }

        [TestMethod]
        public void ConstructorWithRelationParameterShouldSetCorrectProperties()
        {
            // arrange
            const string relation = "relation";

            // act
            var registration = new HateoasRegistration<TestModel>(relation, null);

            // assert
            Assert.AreEqual(relation, registration.Relation);
            Assert.IsNull(registration.ArgumentDefinitions);
            Assert.IsFalse(registration.IsCollection);
        }

        [TestMethod]
        public void ConstructorWithRelationIdAndSpecificationParametersShouldSetCorrectProperties()
        {
            // arrange
            const string relation = "relation";
            Expression<Func<TestModel, object>> identityDefinition = m => m.Id;

            // act
            var registration = new HateoasRegistration<TestModel>(relation, new [] { identityDefinition }, null);

            // assert
            Assert.AreEqual(relation, registration.Relation);
            Assert.AreEqual(identityDefinition, registration.ArgumentDefinitions[0]);
            Assert.IsFalse(registration.IsCollection);
        }

        [TestMethod]
        public void ConstructorWithRelationIdSpecificationAndCollectionFlagParametersShouldSetCorrectProperties()
        {
            // arrange
            const string relation = "relation";
            Expression<Func<TestModel, object>> identityDefinition = m => m.Id;

            // act
            var registration = new HateoasRegistration<TestModel>(relation, new [] { identityDefinition }, null, true);

            // assert
            Assert.AreEqual(relation, registration.Relation);
            Assert.AreEqual(identityDefinition, registration.ArgumentDefinitions[0]);
            Assert.IsTrue(registration.IsCollection);
        }

        [TestMethod]
        public void IHateoasRegistrationExpressionShouldSetExpressionProperty()
        {
            // arrange
            var registration = new HateoasRegistration<TestModel>(null);

            var expressionMock = new Mock<IHateoasExpression<TestModel>>(MockBehavior.Strict);
            var registrationAsIHateoasRegistration = (IHateoasRegistration)registration;

            registrationAsIHateoasRegistration.Expression = expressionMock.Object;
            var assignedEpression = registrationAsIHateoasRegistration.Expression;
            
            // assert
            assignedEpression.Should().NotBeNull().And.Be(expressionMock.Object);
            registration.Expression.Should().NotBeNull().And.Be(expressionMock.Object);
        }

        #region Internal test objects
        // ReSharper disable ClassNeverInstantiated.Local
        // ReSharper disable UnusedAutoPropertyAccessor.Local

        public class TestModel
        {
            public Guid Id { get; set; }
        }

        // ReSharper restore UnusedAutoPropertyAccessor.Local
        // ReSharper restore ClassNeverInstantiated.Local
        #endregion
    }
}