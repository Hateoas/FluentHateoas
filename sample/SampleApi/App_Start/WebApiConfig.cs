using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using FluentHateoas;
using FluentHateoas.Handling;
using FluentHateoas.Handling.Handlers;
using FluentHateoas.Registration;

namespace SampleApi
{
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            Hateoas.Startup<SomeRegistrationClass>(config);

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
