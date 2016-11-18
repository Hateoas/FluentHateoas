using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FluentHateoas.Builder.Model;
using FluentHateoas.Handling;
using FluentHateoas.Interfaces;

namespace FluentHateoas.Builder.Handlers
{
    public class TemplateArgumentsProcessor : ArgumentProcessor
    {
        public override bool CanProcess<TModel>(IHateoasRegistration<TModel> registration, ILinkBuilder linkBuilder)
        {
            return registration.Expression.TemplateParameters != null && registration.Expression.TemplateParameters.Any();
        }

        public override bool Process<TModel>(IHateoasRegistration<TModel> registration, ILinkBuilder linkBuilder, object data)
        {
            var templateArguments = registration.Expression.TemplateParameters ?? new List<LambdaExpression>();
            var templateArgumentIndex = 0;
            foreach (var templateArgument in templateArguments)
            {
                var memberExpression = templateArgument.Body as MemberExpression ??
                                       (MemberExpression)((UnaryExpression)templateArgument.Body).Operand;
                var member = memberExpression.Member;

                var keyOrigin = GetKey(data, member, registration.IsCollection);
                var key = templateArgumentIndex == 0 && !linkBuilder.Arguments.Any() ? "id" : keyOrigin;

                linkBuilder.Arguments.Add(key, CreateTemplateArgument(key, keyOrigin, ((PropertyInfo)member).PropertyType));
                templateArgumentIndex++;
            }

            return true;
        }

        private static Argument CreateTemplateArgument(string name, string origin, Type type)
        {
            return new Argument
            {
                Name = name,
                Origin = origin,
                Type = type,
                Value = $"{{{name}}}",
                IsTemplateArgument = true
            };
        }
    }
}