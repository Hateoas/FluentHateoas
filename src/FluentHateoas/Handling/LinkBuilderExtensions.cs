using System;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using FluentHateoas.Helpers;

namespace FluentHateoas.Handling
{
    public static class LinkBuilderExtensions
    {
        public static IHateoasLink Build(this LinkBuilder source)
        {
            var result = new HateoasLink
            {
                Relation = source.Relation
            };

            if (source.IsTemplate)
                result.Template = source.GetPathAsTemplate();
            else
                result.LinkPath = source.GetPath();

            if (source.Method != HttpMethod.Get)
                result.Method = source.Method.ToString();

            if (source.Command != null)
            {
                result.Command = source.Command;
            }

            return result;
        }

        public static string GetPathAsTemplate(this LinkBuilder source)
        {
            return RouteFromMethod(source.Action);
        }

        public static string GetPath(this LinkBuilder source)
        {
            return RouteFromMethod(source.Action)
                .HaackFormat((object)source.Arguments)
                .HaackFormat(source.Data);
        }

        private static string RouteFromMethod(MethodInfo methodInfo)
        {
            var apiPrefix = "/api/"; // todo: Should come from default of given route

            var controllerAttribute = methodInfo.DeclaringType.GetCustomAttribute<RoutePrefixAttribute>();
            var actionAttribute = methodInfo.GetCustomAttribute<RouteAttribute>();

            var hasPrefix = (controllerAttribute != null && !string.IsNullOrWhiteSpace(controllerAttribute.Prefix));
            var hasTemplate = (actionAttribute != null && !string.IsNullOrWhiteSpace(actionAttribute.Template));

            var controllerType = methodInfo.DeclaringType;
            var controllerTypeName = controllerType == null ? string.Empty : controllerType.Name;

            var prefix = !hasPrefix
                ? apiPrefix + controllerTypeName.Substring(0, controllerTypeName.IndexOf("Controller", StringComparison.Ordinal)).ToLower()
                : apiPrefix + controllerAttribute.Prefix;

            if (hasTemplate)
                return string.Format("{0}/{1}", prefix, actionAttribute.Template);

            var parameters = GetParameterString(methodInfo);
            return string.Format("{0}{1}", prefix, string.IsNullOrWhiteSpace(parameters) ? string.Empty : parameters);
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