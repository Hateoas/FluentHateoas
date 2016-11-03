using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Net.Http;
using System.Reflection;
using FluentAssertions;
using FluentHateoas.Builder.Model;
using FluentHateoas.Helpers;
using FluentHateoasTest.Assets.Controllers;
using FluentHateoasTest.Assets.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

// ReSharper disable InvokeAsExtensionMethod

namespace FluentHateoasTest.Helpers
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ExpressionHelperTest
    {
        [TestMethod]
        [TestCategory("UnitTest")]
        public void ToExpandoShouldReturnExpandoWithObjectProperties()
        {
            // arrange
            var person = new Person { Id = Guid.NewGuid(), Lastname = "LastName" };
            var expression = new Expression<Func<Person, object>>[]
            {
                p => p.Id,
                p => p.Lastname
            };

            // act
            var expando = ExpressionHelper.ToExpando(expression, person);

            // assert
            expando
                .Should().NotBeNull().And
                .HaveCount(2).And
                .ContainKey("id").And
                .ContainKey("lastname");
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public void GetMemberInfoShouldReturnMember()
        {
            // arrange
            Expression<Func<Person, Guid>> expression = p => p.Id;

            // act
            var memberInfo = ExpressionHelper.GetMemberInfo(expression);

            // assert
            memberInfo
                .Should().NotBeNull().And
                .Match((MemberInfo m) => nameof(Person.Id).Equals(m.Name));
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public void GetMemberInfoShouldThrowIfNoMemberExpression()
        {
            // arrange

            // act
            Action action = () => ExpressionHelper.GetMemberInfo((Expression<Func<Person, Guid>>)null);

            // assert
            action.ShouldThrow<ArgumentException>().And
                .Message.Should().Be("expression is not a MemberExpression\r\nParameter name: expression");
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public void GetTargetMethodShouldExtractMethodFromMethodCallFunc()
        {
            // arrange
            Expression<Func<PersonController, Func<object>>> expression = p => p.Get;
            const string relation = "self";
            var httpMethod = HttpMethod.Get;
            var argumentsMock = new Mock<IDictionary<string, Argument>>(MockBehavior.Strict);

            // act
            var targetMethod = ExpressionHelper.GetTargetMethod(expression, relation, httpMethod, argumentsMock.Object);

            // assert
            targetMethod
                .Should().NotBeNull().And
                .Match(a => "Get".Equals(a.Name) && typeof(PersonController) == a.DeclaringType);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public void GetTargetMethodShouldExtractMethodFromDirectMethodCall()
        {
            // arrange
            Expression<Func<PersonController, object>> expression = p => p.Get();
            const string relation = "self";
            var httpMethod = HttpMethod.Get;
            var argumentsMock = new Mock<IDictionary<string, Argument>>(MockBehavior.Strict);

            // act
            var targetMethod = ExpressionHelper.GetTargetMethod(expression, relation, httpMethod, argumentsMock.Object);

            // assert
            targetMethod
                .Should().NotBeNull().And
                .Match(a => "Get".Equals(a.Name) && typeof(PersonController) == a.DeclaringType);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public void GetTargetMethodShouldFailToExtractExpressionWhichIsNoMethodExpression()
        {
            // arrange
            Expression<Func<Person, object>> expression = p => p.Id;
            const string relation = "self";
            var httpMethod = HttpMethod.Get;
            var argumentsMock = new Mock<IDictionary<string, Argument>>(MockBehavior.Strict);

            // act
            Action action = () => ExpressionHelper
                .GetTargetMethod(expression, relation, httpMethod, argumentsMock.Object);

            // assert
            action
                .ShouldThrow<InvalidOperationException>().And
                .Message.Should().Be("Expression is not of type MethodCallExpression");
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public void GetTargetMethodShouldFailToExtractInvalidExpression()
        {
            // arrange
            Expression<Func<PersonController, PersonController>> expression = p => p;

            const string relation = "self";
            var httpMethod = HttpMethod.Get;
            var argumentsMock = new Mock<IDictionary<string, Argument>>(MockBehavior.Strict);

            // act
            Action action = () => ExpressionHelper
                .GetTargetMethod(expression, relation, httpMethod, argumentsMock.Object);

            // assert
            action
                .ShouldThrow<NotImplementedException>().And
                .Message.Should().Be("The method or operation is not implemented.");
        }
    }
}