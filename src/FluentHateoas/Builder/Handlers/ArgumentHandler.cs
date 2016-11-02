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
        ///     .Register&lt;Session&gt;(p => p.MenuId)
        ///     .Use&lt;MenuController&gt;()
        /// 
        /// MenuId becomes Id in a later process
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="registration"></param>
        /// <param name="linkBuilder"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public override void ProcessInternal<TModel>(IHateoasRegistration<TModel> registration, ILinkBuilder linkBuilder, object data)
        {
            if (!ProcessIdFromExpression(registration, linkBuilder, data))
            {
                ProcessArgumentDefinitions(registration, linkBuilder, data);
            }

            ProcessTemplateArguments(registration, linkBuilder, data);
        }

        private bool ProcessIdFromExpression<TModel>(IHateoasRegistration<TModel> registration, ILinkBuilder linkBuilder, object data)
        {
            if (registration.Expression.IdFromExpression == null) return false;

            var compiledExpression = registration.Expression.IdFromExpression.Compile();
            var providerType = registration.Expression.IdFromExpression.Parameters[0].Type;
            var provider = _dependencyResolver.GetService(providerType);
            var result = compiledExpression.DynamicInvoke(provider, data);
            linkBuilder.Arguments.Add("id", CreateArgument("id", result.GetType(), result));

            return true;
        }

        private static void ProcessArgumentDefinitions<TModel>(IHateoasRegistration<TModel> registration, ILinkBuilder linkBuilder, object data)
        {
            if (registration.ArgumentDefinitions == null) return;

            var argumentDefinitionIndex = 0;
            foreach (var argumentDefinition in registration.ArgumentDefinitions)
            {
                // Add the first argument as 'id' so it always can be used as named property 'id'
                var key = argumentDefinitionIndex == 0
                    ? "id"
                    : GetKey(data,
                        ((MemberExpression) ((UnaryExpression) argumentDefinition.Body).Operand).Member);
                var value = argumentDefinition.Compile().DynamicInvoke(data);

                linkBuilder.Arguments.Add(key, CreateArgument(key, value.GetType(), value));
                argumentDefinitionIndex++;
            }
        }

        private static void ProcessTemplateArguments(IHateoasRegistration registration, ILinkBuilder linkBuilder, object data)
        {
            var templateArguments = registration.Expression.TemplateParameters ?? new List<LambdaExpression>();
            var templateArgumentIndex = 0;
            foreach (var templateArgument in templateArguments)
            {
                var memberExpression = templateArgument.Body as MemberExpression ??
                                       (MemberExpression) ((UnaryExpression) templateArgument.Body).Operand;
                var member = memberExpression.Member;

                var key = templateArgumentIndex == 0 && !linkBuilder.Arguments.Any()
                    ? "id"
                    : GetKey(data, member, registration.IsCollection);

                linkBuilder.Arguments.Add(key, CreateTemplateArgument(key, ((PropertyInfo) member).PropertyType));
                templateArgumentIndex++;
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

        public override bool CanProcess<TModel>(IHateoasRegistration<TModel> registration, ILinkBuilder resourceBuilder)
        {
            return (registration.ArgumentDefinitions != null && registration.ArgumentDefinitions.Any())
                || (registration.Expression.TemplateParameters != null && registration.Expression.TemplateParameters.Any())
                || registration.Expression.IdFromExpression != null;
        }
    }
}