using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using FluentHateoas.Handling;
using FluentHateoas.Handling.Handlers;

namespace SampleApi
{
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var linkFactory = new LinkFactory();
            var configurationProvider = new ConfigurationProvider(config, linkFactory);
            var responseProvider = new ResponseProvider(configurationProvider);
            var handler = new HateoasHttpHandler(responseProvider);
            config.MessageHandlers.Add(handler); // todo: dependency resolver

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
