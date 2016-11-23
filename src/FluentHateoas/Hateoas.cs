using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dependencies;
using FluentHateoas.Builder.Handlers;
using FluentHateoas.Contracts;
using FluentHateoas.Handling;
using FluentHateoas.Handling.Handlers;
using FluentHateoas.Registration;

// ReSharper disable ArgumentsStyleNamedExpression

namespace FluentHateoas
{
    public static class Hateoas
    {
        public static void Startup<TRegistrationClass>(
            HttpConfiguration configuration,
            IAuthorizationProvider authorizationProvider = null,
            IDependencyResolver dependencyResolver = null)
            where TRegistrationClass : IHateoasRegistrationProfile, new()
        {
            var configurationWrapper = new HttpConfigurationWrapper(configuration);

            Startup<TRegistrationClass>(
                configurationWrapper,
                authorizationProvider,
                dependencyResolver);
        }

        public static void Startup<TRegistrationClass>(
            IHttpConfiguration configuration,
            IAuthorizationProvider authorizationProvider = null,
            IDependencyResolver dependencyResolver = null)
            where TRegistrationClass : IHateoasRegistrationProfile, new()
        {
            Startup(
                new TRegistrationClass(), 
                configuration, 
                authorizationProvider, 
                dependencyResolver);
        }

        public static void Startup<TRegistrationClass>(
            TRegistrationClass registrationClass,
            IHttpConfiguration configuration,
            IAuthorizationProvider authorizationProvider = null,
            IDependencyResolver dependencyResolver = null)
            where TRegistrationClass : IHateoasRegistrationProfile
        {
            var linkBuilderFactory = new LinkBuilderFactory();

            // todo: this is not very clean; user dependencyresolver etc
            if (authorizationProvider == null)
            {
                var httpContextWrapper = new HttpContextWrapper(HttpContext.Current);
                authorizationProvider = new WebApiAuthorizationProvider(httpContextWrapper);
            }

            var idFromExpressionProcessor = new IdFromExpressionProcessor(dependencyResolver);
            var argumentsDefinitionsProcessor = new ArgumentDefinitionsProcessor();
            var templateArgumentsProcessor = new TemplateArgumentsProcessor();

            var linkFactory = new LinkFactory(
                linkBuilderFactory: linkBuilderFactory,
                authorizationProvider: authorizationProvider,
                idFromExpressionProcessor: idFromExpressionProcessor,
                argumentsDefinitionsProcessor: argumentsDefinitionsProcessor,
                templateArgumentsProcessor: templateArgumentsProcessor
            );

            var inMemoryGenericLinksForMethodsCache = new InMemoryCache<int, MethodInfo>();
            var linksForFuncProvider =
                new ConfigurationProviderGetLinksForFuncProvider(inMemoryGenericLinksForMethodsCache);

            var getLinksForMethodCache =
                new InMemoryCache<Type, Func<ConfigurationProvider, object, IEnumerable<IHateoasLink>>>();
            var configurationProvider = new ConfigurationProvider(configuration, linkFactory, linksForFuncProvider,
                getLinksForMethodCache);

            var responseProvider = new ResponseProvider(configurationProvider);
            var handler = new HateoasHttpHandler(responseProvider);
            configuration.MessageHandlers.Add(handler);

            var container = HateoasContainerFactory.Create(configuration);
            registrationClass.Register(container);
        }
    }
}