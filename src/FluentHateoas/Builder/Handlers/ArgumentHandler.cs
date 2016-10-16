using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Dependencies;
using FluentHateoas.Handling;
using FluentHateoas.Helpers;
using FluentHateoas.Interfaces;

namespace FluentHateoas.Builder.Handlers
{
    public class ArgumentHandler : RegistrationLinkHandlerBase
    {
        private readonly IDependencyResolver _dependencyResolver;

        public ArgumentHandler(IDependencyResolver dependencyResolver)
        {
            _dependencyResolver = dependencyResolver;
        }

        public override LinkBuilder Process<TModel>(IHateoasRegistration<TModel> registration, LinkBuilder resourceBuilder, TModel data)
        {
            if (registration.Expression.WithExpression != null)
            {
                var compiledExpression = registration.Expression.WithExpression.Compile();

                var providerType = registration.Expression.WithExpression.Parameters[0].Type;
                var provider = _dependencyResolver.GetService(providerType);
                var expando = new ExpandoObject();
                var expandoDict = (IDictionary<string, object>) expando;
                var result = compiledExpression.DynamicInvoke(provider, data);
                expandoDict.Add("id", result);
                resourceBuilder.Arguments = expando;
            }
            else
            {
                resourceBuilder.Arguments = registration.ArgumentDefinitions.ToExpando(data);
            }

            return base.Process(registration, resourceBuilder, data);
        }

        public override bool CanProcess<TModel>(IHateoasRegistration<TModel> registration, LinkBuilder resourceBuilder)
        {
            return registration.ArgumentDefinitions.Any() || registration.Expression.WithExpression != null;
        }
    }
}