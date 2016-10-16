using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
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
                resourceBuilder.Arguments.Add("id", compiledExpression.DynamicInvoke(provider, data));
            }
            else
            {
                foreach (var expression in registration.ArgumentDefinitions)
                {
                    var id = ((MemberExpression)((UnaryExpression)expression.Body).Operand).Member.Name;
                    resourceBuilder.Arguments.Add(id.Substring(0, 1).ToLowerInvariant() + id.Substring(1), expression.Compile().DynamicInvoke(data));
                }
            }

            return base.Process(registration, resourceBuilder, data);
        }

        public override bool CanProcess<TModel>(IHateoasRegistration<TModel> registration, LinkBuilder resourceBuilder)
        {
            return registration.ArgumentDefinitions.Any() || registration.Expression.WithExpression != null;
        }
    }
}