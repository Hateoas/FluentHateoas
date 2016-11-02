using System.Web.Http;
using System.Web.Http.Dependencies;
using FluentHateoas.Builder.Handlers;
using FluentHateoas.Contracts;
using FluentHateoas.Handling;
using FluentHateoas.Handling.Handlers;
using FluentHateoas.Registration;

namespace FluentHateoas
{
    public static class Hateoas
    {
        public static void Startup<TRegistrationClass>(HttpConfiguration config, IAuthorizationProvider authorizationProvider = null, IDependencyResolver dependencyResolver = null)
            where TRegistrationClass : IHateoasRegistrationProfile, new()
        {
            // todo: this is not very clean; user dependencyresolver etc
            if (authorizationProvider == null)
            {
                authorizationProvider = new WebApiAuthorizationProvider();
            }

            var idFromExpressionProcessor = new IdFromExpressionProcessor(dependencyResolver);
            var argumentsDefinitionsProcessor = new ArgumentDefinitionsProcessor();
            var templateArgumentsProcessor = new TemplateArgumentsProcessor();

            var linkFactory = new LinkFactory(
                authorizationProvider: authorizationProvider,
                idFromExpressionProcessor: idFromExpressionProcessor,
                argumentsDefinitionsProcessor: argumentsDefinitionsProcessor,
                templateArgumentsProcessor: templateArgumentsProcessor
            );
            var configurationProvider = new ConfigurationProvider(config, linkFactory);
            var responseProvider = new ResponseProvider(configurationProvider);
            var handler = new HateoasHttpHandler(responseProvider);
            config.MessageHandlers.Add(handler); // todo: dependency resolver

            var container = HateoasContainerFactory.Create(config);
            var registration = new TRegistrationClass();

            registration.Register(container);
        }
    }
}