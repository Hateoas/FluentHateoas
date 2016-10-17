using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
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

        /// <summary>
        /// Adds the arguments to the builder.
        /// 
        /// Remarkable at first sight might be adding the $$first$$
        /// 
        /// Example:
        /// Session.MenuId = 5;
        /// 
        /// container
        ///     .Register<Session>(p => p.MenuId)
        ///     .Use<MenuController>()
        /// 
        /// MenuId becomes Id in a later process
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="registration"></param>
        /// <param name="resourceBuilder"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public override LinkBuilder Process<TModel>(IHateoasRegistration<TModel> registration, LinkBuilder resourceBuilder, TModel data)
        {
            var arguments = registration.ArgumentDefinitions;
            var templateArguments = registration.Expression.TemplateParameters;

            if (registration.Expression.IdFromExpression != null)
            {
                var compiledExpression = registration.Expression.IdFromExpression.Compile();

                var providerType = registration.Expression.IdFromExpression.Parameters[0].Type;
                var provider = _dependencyResolver.GetService(providerType);
                resourceBuilder.Arguments.Add("id", compiledExpression.DynamicInvoke(provider, data));
            }
            else if (arguments.Any())
            {
                // Add the first argument so it always can be used as named property 'id'
                resourceBuilder.Arguments.Add("id", arguments.First().Compile().DynamicInvoke(data));
                arguments = arguments.Skip(1).ToArray();
            }

            // Handle arguments
            foreach (var expression in arguments)
            {
                var key = GetKey(data, ((MemberExpression)((UnaryExpression)expression.Body).Operand).Member);
                var result = expression.Compile().DynamicInvoke(data);

                resourceBuilder.Arguments.Add(key, result);
            }

            if (templateArguments == null)
                return base.Process(registration, resourceBuilder, data);

            // Handle templates
            foreach (var expression in templateArguments)
            {
                var key = GetKey(data, ((MemberExpression)((UnaryExpression)expression.Body).Operand).Member);
                resourceBuilder.Arguments.Add(key, ":" + key);
            }

            return base.Process(registration, resourceBuilder, data);
        }

        private static string GetKey<TModel>(TModel data, MemberInfo member)
        {
            var keyName = member.Name;
            var key = keyName == "Id"
                ? data.GetType().Name.Substring(0, 1).ToLowerInvariant() + data.GetType().Name.Substring(1) + keyName // PersonInfo.Id becomes personInfoId
                : keyName.Substring(0, 1).ToLowerInvariant() + keyName.Substring(1);
            return key;
        }

        public override bool CanProcess<TModel>(IHateoasRegistration<TModel> registration, LinkBuilder resourceBuilder)
        {
            return registration.ArgumentDefinitions.Any() || registration.Expression.IdFromExpression != null;
        }
    }
}