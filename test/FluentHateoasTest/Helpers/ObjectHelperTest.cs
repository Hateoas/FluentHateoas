using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using FluentHateoas.Helpers;
using FluentHateoasTest.Assets.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
// ReSharper disable InvokeAsExtensionMethod

namespace FluentHateoasTest.Helpers
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ObjectHelperTest
    {
        [TestMethod]
        [TestCategory("UnitTest")]
        public void IsOrImplementsIEnumerableShouldReturnTrueForListOfTModel()
        {
            // arrange
            var list = new List<Person> { new Person { Lastname = "LastName" } };

            // act
            var result = ObjectHelper.IsOrImplementsIEnumerable(list);

            // assert
            result.Should().Be(true);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public void IsOrImplementsIEnumerableShouldReturnFalseForSingleOfTModel()
        {
            // arrange
            var person = new Person { Lastname = "LastName" };

            // act
            var result = ObjectHelper.IsOrImplementsIEnumerable(person);

            // assert
            result.Should().Be(false);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public void MaterialzeShouldMaterializeWhereSelectListToListOfTModel()
        {
            // arrange
            var persons = new List<Person> {new Person {Lastname = "LastName"}};
            var list = persons.Select(p => p);

            // act
            var result = ObjectHelper.Materialize(list);

            // assert
            result
                .Should().NotBeNull().And
                .BeOfType<List<Person>>().And
                .NotBe(list);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public void MaterialzeShouldNotMaterializeTModel()
        {
            // arrange
            var person = new Person { Lastname = "LastName" };

            // act
            var result = ObjectHelper.Materialize(person);

            // assert
            result
                .Should().NotBeNull().And
                .BeOfType<Person>().And
                .Be(person);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public void MaterialzeShouldNotMaterializeListOfTModel()
        {
            // arrange
            var persons = new List<Person> { new Person { Lastname = "LastName" } };

            // act
            var result = ObjectHelper.Materialize(persons);

            // assert
            result
                .Should().NotBeNull().And
                .BeOfType<List<Person>>().And
                .Be(persons);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public void IsOrImplementsIEnumerableShouldNotValidateSimpleTypeAsEnumerable()
        {
            typeof(string).IsOrImplementsIEnumerable().Should().BeFalse();
            typeof(String).IsOrImplementsIEnumerable().Should().BeFalse();
            typeof(int).IsOrImplementsIEnumerable().Should().BeFalse();
            typeof(Int32).IsOrImplementsIEnumerable().Should().BeFalse();
            typeof(Int64).IsOrImplementsIEnumerable().Should().BeFalse();
            typeof(long).IsOrImplementsIEnumerable().Should().BeFalse();
            typeof(decimal).IsOrImplementsIEnumerable().Should().BeFalse();
            typeof(double).IsOrImplementsIEnumerable().Should().BeFalse();
            typeof(Double).IsOrImplementsIEnumerable().Should().BeFalse();
        }
        }
}