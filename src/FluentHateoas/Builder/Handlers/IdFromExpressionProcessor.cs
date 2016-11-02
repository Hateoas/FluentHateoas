using System.Web.Http.Dependencies;
using FluentHateoas.Builder.Model;
using FluentHateoas.Handling;
using FluentHateoas.Interfaces;

namespace FluentHateoas.Builder.Handlers
{
    public class IdFromExpressionProcessor : ArgumentProcessor
    {
        private readonly IDependencyResolver _dependencyResolver;

        public IdFromExpressionProcessor(IDependencyResolver dependencyResolver)
        {
            _dependencyResolver = dependencyResolver;
        }

        public override bool CanProcess<TModel>(IHateoasRegistration<TModel> registration, ILinkBuilder linkBuilder)
        {
            return registration.Expression.IdFromExpression != null;
        }

        public override bool Process<TModel>(IHateoasRegistration<TModel> registration, ILinkBuilder linkBuilder, object data)
        {
            if (registration.Expression.IdFromExpression == null)
                return false;

            var compiledExpression = registration.Expression.IdFromExpression.Compile();
            var providerType = registration.Expression.IdFromExpression.Parameters[0].Type;
            var provider = _dependencyResolver.GetService(providerType);
            var value = compiledExpression.DynamicInvoke(provider, data);

            linkBuilder.Arguments.Add("id", new Argument {Name = "id", Type = value.GetType(), Value = value});

            return true;
        }
    }
}