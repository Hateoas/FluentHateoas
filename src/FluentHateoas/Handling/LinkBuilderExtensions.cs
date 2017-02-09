using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentHateoas.Builder.Model;
using FluentHateoas.Helpers;

namespace FluentHateoas.Handling
{
    public static class LinkBuilderExtensions
    {
        public static string GetPathAsTemplate(this ILinkBuilder linkBuilder)
        {
            return RouteFromMethod(linkBuilder);
        }

        public static string GetPath(this ILinkBuilder linkBuilder)
        {
            return RouteFromMethod(linkBuilder)
                .FormatArguments(linkBuilder.Arguments);
            //.HaackFormat(linkBuilder.Data);
        }

        public static string FormatArguments(this string source, IDictionary<string, Argument> dict)
        {
            return dict.Keys.Aggregate(source, (current, key) => current.Replace($"{{{key}}}", dict[key].Value.ToString()));
        }

        private static string RouteFromMethod(ILinkBuilder linkBuilder)
        {
            var apiPrefixSetting = ConfigurationKeys.ApiPrefix;
            var apiPrefix = string.IsNullOrWhiteSpace(apiPrefixSetting)
                ? "/"
                : $"/{apiPrefixSetting}";

            var controllerAttribute = linkBuilder.Action.DeclaringType.GetCustomAttribute<System.Web.Http.RoutePrefixAttribute>();
            var actionAttribute = linkBuilder.Action.GetCustomAttribute<System.Web.Http.RouteAttribute>();

            var hasPrefix = !string.IsNullOrWhiteSpace(controllerAttribute?.Prefix);
            var hasTemplate = !string.IsNullOrWhiteSpace(actionAttribute?.Template);

            string prefix;

            if (!hasPrefix)
            {
                var controllerType = linkBuilder.Action.DeclaringType;
                var controllerTypeName = controllerType?.Name ?? string.Empty;

                prefix = $"{apiPrefix}/" + controllerTypeName.Substring(0, controllerTypeName.IndexOf("Controller", StringComparison.Ordinal)).ToLower();
            }
            else
            {
                prefix = $"{apiPrefix}/" + controllerAttribute.Prefix;
            }


            if (!hasTemplate)
                return $"{prefix}{GetParameterString(linkBuilder)?.Trim()}";

            return actionAttribute.Template[0] != '{' 
                ? $"{apiPrefix}/{actionAttribute.Template}" 
                : $"{prefix}/{actionAttribute.Template}";
        }

        private static string GetParameterString(ILinkBuilder linkBuilder)
        {
            var parameters = linkBuilder.Action.GetParameters();
            var parameterString = parameters.SingleOrDefault(p => p.Name == "id");
            if (parameterString == null)
                return string.Empty;

            return $"/{{{linkBuilder.Arguments["id"].Origin ?? linkBuilder.Arguments["id"].Name}}}";
        }
    }
}