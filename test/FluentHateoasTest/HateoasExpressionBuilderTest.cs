// ReSharper disable RedundantTypeArgumentsOfMethod

using FluentHateoas.Contracts;
using FluentHateoas.Interfaces;
using Moq;

namespace FluentHateoasTest
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http.Controllers;

    using FluentHateoas.Registration;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    [ExcludeFromCodeCoverage]
    public class HateoasExpressionBuilderTest
    {
        [TestMethod]
        public void ConstructorShouldSaveRegistration()
        {
            // arrange
            var containerMock = new Mock<IHateoasContainer>(MockBehavior.Strict);
            const string Relation = "relation";
            Expression<Func<TestModel, object>> identityDefinition = m => m.Id;
            var registration = new HateoasRegistration<TestModel>(Relation, new [] { identityDefinition }, containerMock.Object);

            // act
            var builder = new ExpressionBuilder<TestModel>(registration);
            var expression = builder.GetExpression();

            // assert
            Assert.IsNotNull(expression);

            Assert.AreEqual(Relation, expression.Relation);
            Assert.AreEqual(identityDefinition, expression.ArgumentDefinitions[0]);
            Assert.IsFalse(expression.IsCollection);
        }

        [TestMethod]
        public void GetShouldSaveControllerAndSetHttpMethodGet()
        {
            // arrange
            var containerMock = new Mock<IHateoasContainer>(MockBehavior.Strict);
            containerMock.Setup(c => c.Update(It.IsAny<IHateoasRegistration>()));
            var registration = new HateoasRegistration<TestModel>(null, null, containerMock.Object);
            var builder = new ExpressionBuilder<TestModel>(registration);

            // act
            builder.Get<TestModelController>();
            var expression = builder.GetExpression();

            // assert
            Assert.IsNotNull(expression);

            Assert.AreEqual(typeof(TestModelController), expression.Controller);
            Assert.AreEqual(HttpMethod.Get, expression.HttpMethod);
            Assert.IsNull(expression.Action);
            containerMock.Verify(c => c.Update(It.IsAny<IHateoasRegistration>()), Times.Once);
        }

        [TestMethod]
        public void GetWithCustomGetAllActionShouldSaveControllerAndSetHttpMethodGet()
        {
            // arrange
            var containerMock = new Mock<IHateoasContainer>(MockBehavior.Strict);
            containerMock.Setup(c => c.Update(It.IsAny<IHateoasRegistration>()));
            var registration = new HateoasRegistration<TestModel>(null, null, containerMock.Object);
            var builder = new ExpressionBuilder<TestModel>(registration);

            // act
            Expression<Func<TestModelController, Func<IEnumerable<TestModel>>>> getAllExpression = c => c.GetAll;
            builder.Get<TestModelController>(getAllExpression);
            var expression = builder.GetExpression();

            // assert
            Assert.IsNotNull(expression);

            Assert.AreEqual(typeof(TestModelController), expression.Controller);
            Assert.AreEqual(HttpMethod.Get, expression.HttpMethod);
            Assert.AreEqual(getAllExpression, expression.Action);
            containerMock.Verify(c => c.Update(It.IsAny<IHateoasRegistration>()), Times.Once);
        }

        [TestMethod]
        public void GetWithCustomGetAllMethodSelectorShouldSaveControllerAndSetHttpMethodGet()
        {
            // arrange
            var containerMock = new Mock<IHateoasContainer>(MockBehavior.Strict);
            containerMock.Setup(c => c.Update(It.IsAny<IHateoasRegistration>()));
            var registration = new HateoasRegistration<TestModel>(null, null, containerMock.Object);
            var builder = new ExpressionBuilder<TestModel>(registration);

            // act
            Expression<Func<TestModelController, IEnumerable<TestModel>>> getAllExpression = c => c.GetAll();
            builder.Get<TestModelController>(getAllExpression);
            var expression = builder.GetExpression();

            // assert
            Assert.IsNotNull(expression);

            Assert.AreEqual(typeof(TestModelController), expression.Controller);
            Assert.AreEqual(HttpMethod.Get, expression.HttpMethod);
            Assert.AreEqual(getAllExpression, expression.Action);
            containerMock.Verify(c => c.Update(It.IsAny<IHateoasRegistration>()), Times.Once);
        }

        [TestMethod]
        public void GetWithCustomGetSingleMethodSelectorShouldSaveControllerAndSetHttpMethodGet()
        {
            // arrange
            var containerMock = new Mock<IHateoasContainer>(MockBehavior.Strict);
            containerMock.Setup(c => c.Update(It.IsAny<IHateoasRegistration>()));
            var registration = new HateoasRegistration<TestModel>(null, null, containerMock.Object);
            var builder = new ExpressionBuilder<TestModel>(registration);

            // act
            Expression<Func<TestModelController, Func<Guid, TestModel>>> getSingleExpression = c => c.GetSingle;
            builder.Get<TestModelController>(getSingleExpression);
            var expression = builder.GetExpression();

            // assert
            Assert.IsNotNull(expression);

            Assert.AreEqual(typeof(TestModelController), expression.Controller);
            Assert.AreEqual(HttpMethod.Get, expression.HttpMethod);
            Assert.AreEqual(getSingleExpression, expression.Action);
            containerMock.Verify(c => c.Update(It.IsAny<IHateoasRegistration>()), Times.Once);
        }

        [TestMethod]
        public void PostShouldSaveControllerAndSetHttpMethodPost()
        {
            // arrange
            var containerMock = new Mock<IHateoasContainer>(MockBehavior.Strict);
            containerMock.Setup(c => c.Update(It.IsAny<IHateoasRegistration>()));
            var registration = new HateoasRegistration<TestModel>(null, null, containerMock.Object);
            var builder = new ExpressionBuilder<TestModel>(registration);

            // act
            builder.Post<TestModelController>();
            var expression = builder.GetExpression();

            // assert
            Assert.IsNotNull(expression);

            Assert.AreEqual(typeof(TestModelController), expression.Controller);
            Assert.AreEqual(HttpMethod.Post, expression.HttpMethod);
            Assert.IsNull(expression.Action);
            containerMock.Verify(c => c.Update(It.IsAny<IHateoasRegistration>()), Times.Once);
        }

        [TestMethod]
        public void PutShouldSaveControllerAndSetHttpMethodPut()
        {
            // arrange
            var containerMock = new Mock<IHateoasContainer>(MockBehavior.Strict);
            containerMock.Setup(c => c.Update(It.IsAny<IHateoasRegistration>()));
            var registration = new HateoasRegistration<TestModel>(null, null, containerMock.Object);
            var builder = new ExpressionBuilder<TestModel>(registration);

            // act
            builder.Put<TestModelController>();
            var expression = builder.GetExpression();

            // assert
            Assert.IsNotNull(expression);

            Assert.AreEqual(typeof(TestModelController), expression.Controller);
            Assert.AreEqual(HttpMethod.Put, expression.HttpMethod);
            Assert.IsNull(expression.Action);
            containerMock.Verify(c => c.Update(It.IsAny<IHateoasRegistration>()), Times.Once);
        }

        [TestMethod]
        public void DeleteShouldSaveControllerAndSetHttpMethodDelete()
        {
            // arrange
            var containerMock = new Mock<IHateoasContainer>(MockBehavior.Strict);
            containerMock.Setup(c => c.Update(It.IsAny<IHateoasRegistration>()));
            var registration = new HateoasRegistration<TestModel>(null, null, containerMock.Object);
            var builder = new ExpressionBuilder<TestModel>(registration);

            // act
            builder.Delete<TestModelController>();
            var expression = builder.GetExpression();

            // assert
            Assert.IsNotNull(expression);

            Assert.AreEqual(typeof(TestModelController), expression.Controller);
            Assert.AreEqual(HttpMethod.Delete, expression.HttpMethod);
            Assert.IsNull(expression.Action);
            containerMock.Verify(c => c.Update(It.IsAny<IHateoasRegistration>()), Times.Once);
        }

        [TestMethod]
        public void AsCollectionShouldSetCollectionProperty()
        {
            // arrange
            var containerMock = new Mock<IHateoasContainer>(MockBehavior.Strict);
            containerMock.Setup(c => c.Update(It.IsAny<IHateoasRegistration>()));
            var registration = new HateoasRegistration<TestModel>(null, null, containerMock.Object);
            var builder = new ExpressionBuilder<TestModel>(registration);

            var fluentResult = builder.Get<TestModelController>();
            containerMock.ResetCalls();

            // act
            fluentResult.AsCollection();
            var expression = builder.GetExpression();

            // assert
            Assert.IsNotNull(expression);
            Assert.IsTrue(expression.Collection);
            containerMock.Verify(c => c.Update(It.IsAny<IHateoasRegistration>()), Times.Once);
        }

        [TestMethod]
        public void AsTemplateShouldSetTemplateProperty()
        {
            // arrange
            var containerMock = new Mock<IHateoasContainer>(MockBehavior.Strict);
            containerMock.Setup(c => c.Update(It.IsAny<IHateoasRegistration>()));
            var registration = new HateoasRegistration<TestModel>(null, null, containerMock.Object);
            var builder = new ExpressionBuilder<TestModel>(registration);

            var fluentResult = builder.Get<TestModelController>();
            containerMock.ResetCalls();

            // act
            fluentResult.AsTemplate();
            var expression = builder.GetExpression();

            // assert
            Assert.IsNotNull(expression);
            Assert.IsTrue(expression.Template);
            Assert.IsNull(expression.TemplateParameters);
            containerMock.Verify(c => c.Update(It.IsAny<IHateoasRegistration>()), Times.Once);
        }

        [TestMethod]
        public void AsTemplateWithParametersShouldSetTemplateProperty()
        {
            // arrange
            var containerMock = new Mock<IHateoasContainer>(MockBehavior.Strict);
            containerMock.Setup(c => c.Update(It.IsAny<IHateoasRegistration>()));
            var registration = new HateoasRegistration<TestModel>(null, null, containerMock.Object);
            var builder = new ExpressionBuilder<TestModel>(registration);

            var fluentResult = builder.Get<TestModelController>();
            containerMock.ResetCalls();

            // act
            Expression<Func<TestModel, object>> idExpression = m => m.Id;
            Expression<Func<TestModel, object>> nameExpression = m => m.Name;
            fluentResult.AsTemplate(idExpression, nameExpression);
            var expression = builder.GetExpression();

            // assert
            Assert.IsNotNull(expression);
            Assert.IsTrue(expression.Template);
            var parameters = expression.TemplateParameters.ToList();
            Assert.AreEqual(2, parameters.Count);
            Assert.AreEqual(idExpression, parameters[0]);
            Assert.AreEqual(nameExpression, parameters[1]);
            containerMock.Verify(c => c.Update(It.IsAny<IHateoasRegistration>()), Times.Once);
        }

        [TestMethod]
        public void WhenShouldSetWhenProperty()
        {
            // arrange
            var containerMock = new Mock<IHateoasContainer>(MockBehavior.Strict);
            containerMock.Setup(c => c.Update(It.IsAny<IHateoasRegistration>()));
            var registration = new HateoasRegistration<TestModel>(null, null, containerMock.Object);
            var builder = new ExpressionBuilder<TestModel>(registration);

            var fluentResult = builder.Get<TestModelController>();
            containerMock.ResetCalls();

            // act
            Expression<Func<ITestModelProvider, TestModel, bool>> hasNextExpression = (provider, testModel) => provider.HasNextId(testModel);
            fluentResult.When<ITestModelProvider>(hasNextExpression);
            var expression = builder.GetExpression();

            // assert
            Assert.IsNotNull(expression);
            Assert.AreEqual(hasNextExpression, expression.WhenExpression);
            containerMock.Verify(c => c.Update(It.IsAny<IHateoasRegistration>()), Times.Once);
        }

        [TestMethod]
        public void WithShouldSetWithProperty()
        {
            // arrange
            var containerMock = new Mock<IHateoasContainer>(MockBehavior.Strict);
            containerMock.Setup(c => c.Update(It.IsAny<IHateoasRegistration>()));
            var registration = new HateoasRegistration<TestModel>(null, null, containerMock.Object);
            var builder = new ExpressionBuilder<TestModel>(registration);

            var fluentResult = builder
                .Get<TestModelController>()
                .When<ITestModelProvider>((provider, testModel) => provider.HasNextId(testModel));
            containerMock.ResetCalls();

            // act
            Expression<Func<ITestModelProvider, TestModel, object>> getNextExpression = (provider, testModel) => provider.GetNextId(testModel);
            fluentResult.IdFrom<ITestModelProvider>(getNextExpression);
            var expression = builder.GetExpression();

            // assert
            Assert.IsNotNull(expression);
            Assert.AreEqual(getNextExpression, expression.IdFromExpression);
            containerMock.Verify(c => c.Update(It.IsAny<IHateoasRegistration>()), Times.Once);
        }

        [TestMethod]
        public void WithCommandShouldSaveCommandType()
        {
            // arrange
            var containerMock = new Mock<IHateoasContainer>(MockBehavior.Strict);
            containerMock.Setup(c => c.Update(It.IsAny<IHateoasRegistration>()));
            var registration = new HateoasRegistration<TestModel>(null, null, containerMock.Object);

            var fluentResult = 
                new ExpressionBuilder<TestModel>(registration)
                    .Post<TestModelController>();
            containerMock.ResetCalls();

            // act
            fluentResult.WithCommand<PostCommand>();
            var expression = fluentResult.GetExpression();

            // assert
            Assert.IsNotNull(expression);

            Assert.AreEqual(typeof(PostCommand), expression.Command);
            Assert.IsNull(expression.CommandFactory);
            containerMock.Verify(c => c.Update(It.IsAny<IHateoasRegistration>()), Times.Once);
        }

        [TestMethod]
        public void WithCommandUsingFactoryExpressionShouldSaveCommandTypeAndCommandExpression()
        {
            // arrange
            var containerMock = new Mock<IHateoasContainer>(MockBehavior.Strict);
            containerMock.Setup(c => c.Update(It.IsAny<IHateoasRegistration>()));
            var registration = new HateoasRegistration<TestModel>(null, null, containerMock.Object);

            var fluentResult =
                new ExpressionBuilder<TestModel>(registration)
                    .Post<TestModelController>();
            containerMock.ResetCalls();

            // act
            Expression<Func<PostCommandFactory, object>> commandFactory = factory => factory.Create();
            fluentResult.WithCommand<PostCommandFactory>(commandFactory);
            var expression = fluentResult.GetExpression();

            // assert
            Assert.IsNotNull(expression);

            Assert.IsNull(expression.Command);
            Assert.IsNotNull(expression.CommandFactory);
            Assert.AreEqual(commandFactory, expression.CommandFactory);
            containerMock.Verify(c => c.Update(It.IsAny<IHateoasRegistration>()), Times.Once);
        }

        #region Internal test objects
        // ReSharper disable ClassNeverInstantiated.Local
        // ReSharper disable MemberCanBeMadeStatic.Local
        // ReSharper disable UnusedAutoPropertyAccessor.Local

        private class TestModel
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
        }

        private class TestModelController : IHttpController
        {
            public IEnumerable<TestModel> GetAll()
            {
                throw new InvalidOperationException();
            }

            public TestModel GetSingle(Guid id)
            {
                throw new InvalidOperationException();
            }

            public Task<HttpResponseMessage> ExecuteAsync(HttpControllerContext controllerContext, CancellationToken cancellationToken)
            {
                throw new InvalidOperationException();
            }
        }

        private interface ITestModelProvider
        {
            bool HasNextId(TestModel testModel);
            object GetNextId(TestModel testModel);
        }

        private class PostCommand
        {
        }

        private class PostCommandFactory
        {
            public object Create()
            {
                throw new InvalidOperationException();
            }
        }

        // ReSharper restore UnusedAutoPropertyAccessor.Local
        // ReSharper restore MemberCanBeMadeStatic.Local
        // ReSharper restore ClassNeverInstantiated.Local
        #endregion
    }
}