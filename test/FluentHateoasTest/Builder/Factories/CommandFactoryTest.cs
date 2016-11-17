using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using FluentHateoas.Attributes;
using FluentHateoas.Builder.Factories;
using FluentHateoas.Builder.Model;
using FluentHateoas.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
// ReSharper disable InvokeAsExtensionMethod

namespace FluentHateoasTest.Builder.Factories
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class CommandFactoryTest
    {
        private Mock<IHateoasExpression> _expression;

        [TestInitialize]
        public void TestInitialize()
        {
            _expression = new Mock<IHateoasExpression>(MockBehavior.Strict);
        }

        [TestMethod]
        public void CreateCommandShouldCreateCommand()
        {
            // arrange
            const string name = "name";
            var type = typeof(TestObject).Name;

            _expression.SetupGet(e => e.Command).Returns(typeof(TestObject));

            // act & assert
            var command = CommandFactory
                .CreateCommand(_expression.Object, name);
            command
                .Should().NotBeNull()
                .And.Match((Command c) => name.Equals(c.Name))
                .And.Match((Command c) => type.Equals(c.Type))
                .And.Match((Command c) => c.Properties.Count == 1);

            command.Properties.First()
                .Should().Match((Property p) => "Int32".Equals(p.Type) &&
                                                nameof(TestObject.Integer).Equals(p.Name) &&
                                                0.Equals(p.Order) &&
                                                true.Equals(p.Required));

            Assert.IsTrue(command.Properties.Any(p =>
            {
                var property = p as IntegerProperty;

                return property != null &&
                       property.Min == 0 &&
                       property.Max == 100;
            }));
        }

        private class TestObject
        {
            // ReSharper disable once UnusedMember.Local Used in unit test 'CreateCommandShouldCreateCommand'!
            [MinValue(0)]
            [MaxValue(100)]
            public int Integer { get; set; }
        }
    }
}
