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
            return RouteFromMethod(linkBuilder.Action);
        }

        public static string GetPath(this ILinkBuilder linkBuilder)
        {
            return RouteFromMethod(linkBuilder.Action)
                .FormatArguments(linkBuilder.Arguments);
            //.HaackFormat(linkBuilder.Data);
        }

        public static string FormatArguments(this string source, IDictionary<string, Argument> dict)
        {
            return dict.Keys.Aggregate(source, (current, key) => current.Replace($"{{{key}}}", dict[key].Value.ToString()));
        }

        private static string RouteFromMethod(MethodInfo methodInfo)
        {
            var apiPrefixSetting = ConfigurationKeys.ApiPrefix;
            var apiPrefix = string.IsNullOrWhiteSpace(apiPrefixSetting)
                ? "/"
                : $"/{apiPrefixSetting}/";

            var controllerAttribute = methodInfo.DeclaringType.GetCustomAttribute<System.Web.Http.RoutePrefixAttribute>();
            var actionAttribute = methodInfo.GetCustomAttribute<System.Web.Http.RouteAttribute>();

            var hasPrefix = !string.IsNullOrWhiteSpace(controllerAttribute?.Prefix);
            var hasTemplate = !string.IsNullOrWhiteSpace(actionAttribute?.Template);

            var controllerType = methodInfo.DeclaringType;
            var controllerTypeName = controllerType == null ? string.Empty : controllerType.Name;

            var prefix = !hasPrefix
                ? apiPrefix + controllerTypeName.Substring(0, controllerTypeName.IndexOf("Controller", StringComparison.Ordinal)).ToLower()
                : apiPrefix + controllerAttribute.Prefix;

            if (hasTemplate)
                return $"{prefix}/{actionAttribute.Template}";

            return $"{prefix}{GetParameterString(methodInfo)?.Trim()}";
        }

        private static string GetParameterString(MethodInfo methodInfo)
        {
            var parameters = methodInfo.GetParameters();
            var parameterString = parameters.Any(p => p.Name == "id")
                ? "/{id}"
                : "";
            return parameterString;
        }
    }
}