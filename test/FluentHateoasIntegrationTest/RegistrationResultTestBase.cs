using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Dependencies;
using System.Web.Http.Hosting;
using FluentAssertions;
using FluentHateoas;
using FluentHateoas.Builder.Handlers;
using FluentHateoas.Contracts;
using FluentHateoas.Handling;
using FluentHateoas.Helpers;
using FluentHateoas.Registration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FluetHateoasIntegrationTest
{
    [ExcludeFromCodeCoverage]
    public class RegistrationResultTestBase
    {
        protected TestRegistrationClass RegistrationClass;

        protected HttpRequestMessage Request;
        protected HttpResponseMessage Response;

        private Mock<IAuthorizationProvider> _authorizationProviderMock;
        private Mock<IDependencyResolver> _dependencyResolver;
        private ILinkFactory _linkFactory;
        private IConfigurationProviderGetLinksForFuncProvider _linksForFuncProvider;
        private ICache<Type, Func<ConfigurationProvider, object, IEnumerable<IHateoasLink>>> _getLinksForMethodCache;
        private ConfigurationProvider _configurationProvider;
        private ResponseProvider _responseProvider;
        private HttpConfiguration _httpConfiguration;
        private HttpConfigurationWrapper _httpConfigurationWrapper;

        [TestInitialize]
        public void BaseTestInitialize()
        {
            // registration
            _httpConfiguration = new HttpConfiguration();
            _httpConfigurationWrapper = new HttpConfigurationWrapper(_httpConfiguration);
            _authorizationProviderMock = new Mock<IAuthorizationProvider>(MockBehavior.Strict);
            _authorizationProviderMock.Setup(a => a.IsAuthorized(It.IsAny<MethodInfo>())).Returns(true);
            _dependencyResolver = new Mock<IDependencyResolver>(MockBehavior.Strict);
            RegistrationClass = new TestRegistrationClass();

            // request & response
            Request = new HttpRequestMessage();
            Request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
            Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, _httpConfigurationWrapper.HttpConfiguration);
            Response = new HttpResponseMessage(HttpStatusCode.OK);

            // handling
            var idFromExpressionProcessor = new IdFromExpressionProcessor(_dependencyResolver.Object);
            var argumentsDefinitionsProcessor = new ArgumentDefinitionsProcessor();
            var templateArgumentsProcessor = new TemplateArgumentsProcessor();

            var inMemoryGenericLinksForMethodsCache = new InMemoryCache<int, MethodInfo>();
            _linksForFuncProvider = new ConfigurationProviderGetLinksForFuncProvider(inMemoryGenericLinksForMethodsCache);

            var linkBuilderFactory = new LinkBuilderFactory();

            _linkFactory = new LinkFactory(
                linkBuilderFactory: linkBuilderFactory,
                authorizationProvider: _authorizationProviderMock.Object,
                idFromExpressionProcessor: idFromExpressionProcessor,
                argumentsDefinitionsProcessor: argumentsDefinitionsProcessor,
                templateArgumentsProcessor: templateArgumentsProcessor
            );

            _getLinksForMethodCache = new InMemoryCache<Type, Func<ConfigurationProvider, object, IEnumerable<IHateoasLink>>>();
            _configurationProvider = new ConfigurationProvider(_httpConfigurationWrapper, _linkFactory, _linksForFuncProvider, _getLinksForMethodCache);
            _responseProvider = new ResponseProvider(_configurationProvider);

        }

        protected async Task<TestResult> RegisterGetAndAssertResponse(bool isHateoasResponse, object data, int linkCount, int commandCount)
        {
            Hateoas.Startup(RegistrationClass, _httpConfigurationWrapper, _authorizationProviderMock.Object, _dependencyResolver.Object);
            var response = _responseProvider.Create(Request, Response);
            var testResult = new TestResult(response);
            await testResult.GetResponseString();

            // assert
            testResult.IsHateoasResponse.Should().Be(isHateoasResponse);
            testResult.HateoasResponse.Data.Should().Be(data);
            testResult.HateoasResponse.Links.Should().HaveCount(linkCount);
            testResult.HateoasResponse.Commands.Should().HaveCount(commandCount);

            return testResult;
        }

        protected class TestRegistrationClass : IHateoasRegistrationProfile
        {
            public Action<IHateoasContainer> Registering;

            public void Register(IHateoasContainer container)
            {
                Registering?.Invoke(container);
            }
        }

        protected class TestResult
        {
            public HttpStatusCode ResponseStatus { get; }
            public HttpResponseMessage Response { get; }
            public ObjectContent Content { get; }
            public bool IsHateoasResponse { get; }
            public HateoasResponse HateoasResponse { get; }
            public string ResponseString { get; private set; }

            public TestResult(HttpResponseMessage response)
            {
                Response = response;
                ResponseStatus = response.StatusCode;

                Content = response.Content as ObjectContent;

                var hateoasResponse = Content?.Value as HateoasResponse;

                if (hateoasResponse == null)
                    return;

                IsHateoasResponse = true;
                HateoasResponse = hateoasResponse;
            }

            public async Task<string> GetResponseString()
            {
                ResponseString = await Content.GetAsString();
                return ResponseString;
            }
        }
    }
}