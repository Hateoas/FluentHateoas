using System.Web.Http;
using FluentHateoas.Contracts;
using FluentHateoas.Handling;
using FluentHateoas.Handling.Handlers;
using FluentHateoas.Registration;

namespace FluentHateoas
{
    public static class Hateoas
    {
        public static void Startup<TRegistrationClass>(HttpConfiguration config) 
            where TRegistrationClass : IHateoasRegistrationProfile, new()
        {
            // todo: this is not very clean; user dependencyresolver etc
            var linkFactory = new LinkFactory();
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