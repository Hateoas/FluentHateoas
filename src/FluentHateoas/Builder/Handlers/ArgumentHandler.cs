using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Http.Dependencies;
using FluentHateoas.Builder.Model;
using FluentHateoas.Handling;
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
        public override void ProcessInternal<TModel>(IHateoasRegistration<TModel> registration, LinkBuilder resourceBuilder, object data)
        {
            var arguments = registration.ArgumentDefinitions;
            var templateArguments = (registration.Expression.TemplateParameters ?? new List<LambdaExpression>()).ToArray();

            if (registration.Expression.IdFromExpression != null)
            {
                var compiledExpression = registration.Expression.IdFromExpression.Compile();

                var providerType = registration.Expression.IdFromExpression.Parameters[0].Type;
                var provider = _dependencyResolver.GetService(providerType);
                var result = compiledExpression.DynamicInvoke(provider, data);
                resourceBuilder.Arguments.Add("id", CreateArgument("id", result.GetType(), result));
            }
            else if (arguments != null && arguments.Any())
            {
                // Add the first argument so it always can be used as named property 'id'
                var result = arguments.First().Compile().DynamicInvoke(data);
                resourceBuilder.Arguments.Add("id", CreateArgument("id", result.GetType(), result));
                arguments = arguments.Skip(1).ToArray();

                // Handle arguments
                foreach (var expression in arguments)
                {
                    var key = GetKey(data, ((MemberExpression)((UnaryExpression)expression.Body).Operand).Member);
                    var invokeResult = expression.Compile().DynamicInvoke(data);

                    resourceBuilder.Arguments.Add(key, CreateArgument(key, invokeResult.GetType(), invokeResult));
                }
            }

            if (!templateArguments.Any()) return;

            // Handle templates
            if (!resourceBuilder.Arguments.Any())
            {
                var first = templateArguments.First();
                var member = first.Body is MemberExpression
                    ? ((MemberExpression)first.Body).Member
                    : ((MemberExpression)((UnaryExpression)first.Body).Operand).Member;

                resourceBuilder.Arguments.Add("id", CreateTemplateArgument("id", ((PropertyInfo)member).PropertyType));

                templateArguments = templateArguments.Skip(1).ToArray();
            }

            foreach (var expression in templateArguments)
            {
                var member = expression.Body is MemberExpression
                    ? ((MemberExpression) expression.Body).Member
                    : ((MemberExpression)((UnaryExpression)expression.Body).Operand).Member;

                var key = GetKey(data, member, registration.IsCollection);
                resourceBuilder.Arguments.Add(key, CreateTemplateArgument(key, ((PropertyInfo)member).PropertyType));
            }
        }

        private static Argument CreateArgument(string name, Type type, object value)
        {
            return new Argument
            {
                Name = name,
                Type = type,
                Value = value
            };
        }

        private static Argument CreateTemplateArgument(string name, Type type)
        {
            return new Argument
            {
                Name = name,
                Type = type,
                Value = $"{{{name}}}",
                IsTemplateArgument = true
            };
        }

        private static string GetKey<TModel>(TModel data, MemberInfo member, bool isCollection = false)
        {
            var keyName = member.Name;
            var valueType = isCollection ? data.GetType().GetGenericArguments()[0] : data.GetType();
            var key = keyName == "Id"
                ? valueType.Name.Substring(0, 1).ToLowerInvariant() + valueType.Name.Substring(1) + keyName // PersonInfo.Id becomes personInfoId
                : keyName.Substring(0, 1).ToLowerInvariant() + keyName.Substring(1);
            return key;
        }

        public override bool CanProcess<TModel>(IHateoasRegistration<TModel> registration, LinkBuilder resourceBuilder)
        {
            return (registration.ArgumentDefinitions != null && registration.ArgumentDefinitions.Any()) 
                || (registration.Expression.TemplateParameters != null && registration.Expression.TemplateParameters.Any())
                || registration.Expression.IdFromExpression != null;
        }
    }
}