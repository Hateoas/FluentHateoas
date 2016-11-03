using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentHateoas.Helpers;

namespace FluentHateoasTest.Helpers
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ObjectHelperTest
    {
        [TestMethod]
        [TestCategory("UnitTest")]
        public void IsOrImplementsIEnumerableShouldValidateEnumerableAsEnumerable()
        {
            var input = new List<object>();

            input.IsOrImplementsIEnumerable().Should().BeTrue();
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