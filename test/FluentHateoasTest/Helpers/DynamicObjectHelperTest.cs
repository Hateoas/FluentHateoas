namespace FluentHateoasTest.Helpers
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using FluentHateoas.Helpers;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    [ExcludeFromCodeCoverage]
    public class DynamicObjectHelperTest
    {
        [TestMethod]
        [TestCategory("UnitTest")]
        public void ToExpandoObjectConvertsDynamicToExpandobjectAndIsUsableAsDynamic()
        {
            // arrange
            const string property1Value = "";
            const int property2Value = 0;
            var sourceDynamicObject = new { Property1 = property1Value, Property2 = property2Value };

            // act
            var destinationExpandoObject = DynamicObjectHelper.ToExpandoObject(sourceDynamicObject);

            // assert
            Assert.IsNotNull(destinationExpandoObject);
            var destinationDynamicObject = (dynamic)destinationExpandoObject;
            Assert.AreEqual(property1Value, destinationDynamicObject.Property1);
            Assert.AreEqual(property2Value, destinationDynamicObject.Property2);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public void ToExpandoObjectConvertsDynamicToExpandobjectAndIsUsableAsIDictionary()
        {
            // arrange
            const string property1Value = "";
            const int property2Value = 0;
            var sourceDynamicObject = new { Property1 = property1Value, Property2 = property2Value };

            // act
            var destinationExpandoObject = DynamicObjectHelper.ToExpandoObject(sourceDynamicObject);

            // assert
            Assert.IsNotNull(destinationExpandoObject);
            var destinationDictionaryObject = (IDictionary<string, object>)destinationExpandoObject;
            Assert.AreEqual(property1Value, destinationDictionaryObject["Property1"]);
            Assert.AreEqual(property2Value, destinationDictionaryObject["Property2"]);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public void ToExpandoObjectConvertsNullDynamicToNullExpandoObject()
        {
            // arrange
            
            // act
            var destinationExpandoObject = DynamicObjectHelper.ToExpandoObject(null);

            // assert
            Assert.IsNull(destinationExpandoObject);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public void ToExpandoObjectReturnsExpandObjectIfDynamicObjectAlreadyIsExpandoObject()
        {
            // arrange
            var sourceDynamicObject = new { };
            var sourceExpandoObject = DynamicObjectHelper.ToExpandoObject(sourceDynamicObject);

            // act
            var destinationExpandoObject = DynamicObjectHelper.ToExpandoObject(sourceExpandoObject);

            // assert
            Assert.IsNotNull(destinationExpandoObject);
            Assert.AreSame(sourceExpandoObject, destinationExpandoObject);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public void HasPropertyObjectIsNullReturnsFalse()
        {
            // arrange

            // act
            var hasProperty = DynamicObjectHelper.HasProperty(null, "Property");

            // assert
            Assert.IsFalse(hasProperty);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public void HasPropertyPropertyExistsOnDynamicObjectReturnsTrue()
        {
            // arrange
            var sourceDynamicObject = new { Property = "Value" };

            // act
            var hasProperty = DynamicObjectHelper.HasProperty(sourceDynamicObject, "Property");

            // assert
            Assert.IsTrue(hasProperty);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public void HasPropertyPropertyNotExistsOnDynamicObjectReturnsFalse()
        {
            // arrange
            var sourceDynamicObject = new { };

            // act
            var hasProperty = DynamicObjectHelper.HasProperty(sourceDynamicObject, "Property");

            // assert
            Assert.IsFalse(hasProperty);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public void HasPropertyPropertyExistsOnExpandoObjectReturnsTrue()
        {
            // arrange
            var sourceDynamicObject = new { Property = "Value" };
            var destinationExpandoObject = DynamicObjectHelper.ToExpandoObject(sourceDynamicObject);

            // act
            var hasProperty = DynamicObjectHelper.HasProperty(destinationExpandoObject, "Property");

            // assert
            Assert.IsTrue(hasProperty);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public void HasPropertyPropertyNotExistsOnExpandoObjectReturnsFalse()
        {
            // arrange
            var sourceDynamicObject = new { };
            var destinationExpandoObject = DynamicObjectHelper.ToExpandoObject(sourceDynamicObject);

            // act
            var hasProperty = DynamicObjectHelper.HasProperty(destinationExpandoObject, "Property");

            // assert
            Assert.IsFalse(hasProperty);
        }
    }
}