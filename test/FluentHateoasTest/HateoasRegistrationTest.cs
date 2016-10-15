namespace FluentHateoasTest
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq.Expressions;

    using FluentHateoas.Registration;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    [ExcludeFromCodeCoverage]
    public class HateoasRegistrationTest
    {
        [TestMethod]
        public void ConstructorWithoutParametersShouldSetCorrectProperties()
        {
            // arrange
            const string Relation = "self";

            // act
            var registration = new HateoasRegistration<TestModel>(null);

            // assert
            Assert.AreEqual(Relation, registration.Relation);
            Assert.IsNull(registration.ArgumentDefinition);
            Assert.IsFalse(registration.IsCollection);
        }

        [TestMethod]
        public void ConstructorWithRelationParameterShouldSetCorrectProperties()
        {
            // arrange
            const string Relation = "relation";

            // act
            var registration = new HateoasRegistration<TestModel>(Relation, null);

            // assert
            Assert.AreEqual(Relation, registration.Relation);
            Assert.IsNull(registration.ArgumentDefinition);
            Assert.IsFalse(registration.IsCollection);
        }

        [TestMethod]
        public void ConstructorWithRelationIdAndSpecificationParametersShouldSetCorrectProperties()
        {
            // arrange
            const string Relation = "relation";
            Expression<Func<TestModel, object>> identityDefinition = m => m.Id;

            // act
            var registration = new HateoasRegistration<TestModel>(Relation, identityDefinition, null);

            // assert
            Assert.AreEqual(Relation, registration.Relation);
            Assert.AreEqual(identityDefinition, registration.ArgumentDefinition);
            Assert.IsFalse(registration.IsCollection);
        }

        [TestMethod]
        public void ConstructorWithRelationIdSpecificationAndCollectionFlagParametersShouldSetCorrectProperties()
        {
            // arrange
            const string Relation = "relation";
            Expression<Func<TestModel, object>> identityDefinition = m => m.Id;

            // act
            var registration = new HateoasRegistration<TestModel>(Relation, identityDefinition, null, true);

            // assert
            Assert.AreEqual(Relation, registration.Relation);
            Assert.AreEqual(identityDefinition, registration.ArgumentDefinition);
            Assert.IsTrue(registration.IsCollection);
        }

        #region Internal test objects
        // ReSharper disable ClassNeverInstantiated.Local
        // ReSharper disable UnusedAutoPropertyAccessor.Local

        private class TestModel
        {
            public Guid Id { get; set; }
        }

        // ReSharper restore UnusedAutoPropertyAccessor.Local
        // ReSharper restore ClassNeverInstantiated.Local
        #endregion
    }
}