using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using FluentAssertions;
using FluentHateoas.Builder.Model;
using FluentHateoas.Helpers;
using FluentHateoasTest.Assets.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

// ReSharper disable InvokeAsExtensionMethod

namespace FluentHateoasTest.Helpers
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class HttpControllerHelperTest
    {
        [TestMethod]
        [TestCategory("UnitTest")]
        public void GetHttpMethodShouldReturnCorrectHttpMethodNamedMethod()
        {
            // arrange
            var methodNames = new[]
            {
                new {name = "Get", method = HttpMethod.Get},
                new {name = "Post", method = HttpMethod.Post},
                new {name = "Put", method = HttpMethod.Put},
                new {name = "Delete", method = HttpMethod.Delete},
            };

            foreach (var pair in methodNames)
            {
                // arrange
                var methodMock = new Mock<MethodInfo>();
                methodMock.SetupGet(m => m.Name).Returns(pair.name);

                // act & assert
                HttpControllerHelper
                    .GetHttpMethod(methodMock.Object)
                    .ShouldBeEquivalentTo(pair.method);
            }
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public void GetHttpMethodShouldReturnHttpMethodGetAsDefaultReturnType()
        {
            // arrange
            var methodMock = new Mock<MethodInfo>();
            methodMock.SetupGet(m => m.Name).Returns("SomeObscureName");

            // act & assert
            HttpControllerHelper
                .GetHttpMethod(methodMock.Object)
                .ShouldBeEquivalentTo(HttpMethod.Get);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public void GetHttpMethodShouldReturnHttpMethodPostForMethodAttributedWithHttpPost()
        {
            // arrange & act & assert
            HttpControllerHelper
                .GetHttpMethod(typeof(PersonController).GetMethod(nameof(PersonController.AddChild)))
                .ShouldBeEquivalentTo(HttpMethod.Post);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public void GetHttpMethodShouldReturnHttpMethodPutForMethodAttributedWithHttpPut()
        {
            // arrange & act & assert
            HttpControllerHelper
                .GetHttpMethod(typeof(PersonController).GetMethod(nameof(PersonController.AttributedPut)))
                .ShouldBeEquivalentTo(HttpMethod.Put);
        }


        [TestMethod]
        [TestCategory("UnitTest")]
        public void GetHttpMethodShouldReturnHttpMethodDeleteForMethodAttributedWithHttpDelete()
        {
            // arrange & act & assert
            HttpControllerHelper
                .GetHttpMethod(typeof(PersonController).GetMethod(nameof(PersonController.AttributedDelete)))
                .ShouldBeEquivalentTo(HttpMethod.Delete);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public void GetRouteTemplateShouldThrowExceptionWhenGivenMethodWithoutDeclaringType()
        {
            // arrange
            var methodMock = new Mock<MethodInfo>();
            methodMock.SetupGet(m => m.Name).Returns("SomeObscureName");

            Action action = () => HttpControllerHelper.GetRouteTemplate(methodMock.Object);

            // act & assert
            action
                .ShouldThrow<NullReferenceException>()
                .And.Message.Should().Be("DeclaringType can't be null");
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public void GetRouteTemplateShouldWorkForUnattributedClassAndUnattributedMethod()
        {
            // arrange 
            var method = typeof(TestController).GetMethod(nameof(TestController.DoSomething));

            // act & assert
            HttpControllerHelper
                .GetRouteTemplate(method)
                .ShouldBeEquivalentTo("test");
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public void GetRouteTemplateShouldWorkForUnattributedClassAndUnattributedMethodWithId()
        {
            // arrange 
            var method = typeof(TestController).GetMethod(nameof(TestController.DoSomethingWithId));

            // act & assert
            HttpControllerHelper
                .GetRouteTemplate(method)
                .ShouldBeEquivalentTo("test/{id}");
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public void GetRouteTemplateShouldWorkForUnattributedClassAndAttributedMethod()
        {
            // arrange 
            var method = typeof(TestController).GetMethod(nameof(TestController.DoSomethingWithAttribute));

            // act & assert
            HttpControllerHelper
                .GetRouteTemplate(method)
                .ShouldBeEquivalentTo("dont-do-anything");
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public void GetRouteTemplateShouldWorkForAttributedClassAndUnattributedMethod()
        {
            // arrange 
            var method = typeof(TestControllerWithAttribute).GetMethod(nameof(TestControllerWithAttribute.DoSomething));

            // act & assert
            HttpControllerHelper
                .GetRouteTemplate(method)
                .ShouldBeEquivalentTo("test");
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public void GetRouteTemplateShouldWorkForAttributedClassAndAttributedMethod()
        {
            // arrange 
            var method = typeof(TestControllerWithAttribute).GetMethod(nameof(TestControllerWithAttribute.DoSomethingWithAttribute));

            // act & assert
            HttpControllerHelper
                .GetRouteTemplate(method)
                .ShouldBeEquivalentTo("test/dont-do-anything");
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public void GetActionShouldThrowExceptionWhenNoMethodFound()
        {
            // arrange
            var method = HttpMethod.Options;
            var source = typeof(TestController);
            var relation = "self";
            var argumentsMock = new Mock<IDictionary<string, Argument>>();

            Action action = () => HttpControllerHelper.GetAction(source, relation, method, argumentsMock.Object);

            // act & assert
            action
                .ShouldThrow<Exception>()
                .And.Message.Should().Be($"No suitable action found for {method} on {source.Name} (relation: {relation})");
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public void GetActionShouldReturnDirectlyIfOnlyOneMethodFound()
        {
            // arrange
            var method = HttpMethod.Post;
            var source = typeof(TestController);
            var relation = "self";
            var argumentsMock = new Mock<IDictionary<string, Argument>>();

            // act & assert
            HttpControllerHelper
                .GetAction(source, relation, method, argumentsMock.Object)
                .Should().NotBeNull()
                .And.Match(m => m.Name == "Post");
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public void GetActionShouldReturnParameterMatchingMethodIfMoreThanOneMethodFound()
        {
            // arrange
            var method = HttpMethod.Delete;
            var source = typeof(TestController);
            var relation = "self";
            //var argumentsMock = new Mock<IDictionary<string, Argument>>();
            //argumentsMock.SetupGet(a=>a.)
            var arguments = new Dictionary<string, Argument>
            {
                {"id", new Argument {Name = "id", IsTemplateArgument = false, Type = typeof(int), Value = 1}}
            };

            // act & assert
            HttpControllerHelper
                .GetAction(source, relation, method, arguments)
                .Should().NotBeNull()
                .And.Match(m => m.Name == "Delete");
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public void GetActionShouldThrowExceptionIfMoreThanOneMethodWithMatchingParametersFound()
        {
            // arrange
            var method = HttpMethod.Get;
            var source = typeof(TestController);
            var relation = "self";
            var arguments = new Dictionary<string, Argument>
            {
                {"id", new Argument {Name = "id", IsTemplateArgument = false, Type = typeof(int), Value = 1}}
            };


            Action action = () => HttpControllerHelper
                .GetAction(source, relation, method, arguments);

            // act & assert
            action
                .ShouldThrow<Exception>()
                .And.Message.Should().Be($"There are multiple actions supporting {method}, try specifying explicit");
        }

        private class TestController
        {
            public void Post(int id)
            {

            }

            public void Delete(int id)
            {

            }

            [HttpDelete]
            public void DeleteWithoutParameter()
            {

            }

            public void DoSomething()
            {

            }

            [Route("dont-do-anything")]
            public void DoSomethingWithAttribute()
            {

            }

            public void DoSomethingWithId(int id)
            {

            }

            public void DoSomethingWithAnotherId(int id)
            {

            }
        }

        [RoutePrefix("test")]
        private class TestControllerWithAttribute
        {
            public void DoSomething()
            {

            }

            [Route("dont-do-anything")]
            public void DoSomethingWithAttribute()
            {

            }
        }
    }
}
