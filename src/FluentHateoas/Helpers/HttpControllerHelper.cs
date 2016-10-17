using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using Microsoft.Build.Tasks.Deployment.Bootstrapper;

namespace FluentHateoas.Helpers
{
    public static class HttpControllerHelper
    {
        public static HttpMethod GetHttpMethod(this MethodInfo method)
        {
            switch (method.Name)
            {
                case "Get":
                    return HttpMethod.Get;

                case "Post":
                    return HttpMethod.Post;

                case "Put":
                    return HttpMethod.Put;

                case "Delete":
                    return HttpMethod.Delete;
            }

            if (method.GetCustomAttribute<HttpPostAttribute>() != null)
                return HttpMethod.Post;

            if (method.GetCustomAttribute<HttpPutAttribute>() != null)
                return HttpMethod.Put;

            if (method.GetCustomAttribute<HttpDeleteAttribute>() != null)
                return HttpMethod.Delete;

            return HttpMethod.Get;
        }

        public static string GetRouteTemplate(this MethodInfo method)
        {
            var methodAttribute = method.GetCustomAttribute<RouteAttribute>();
            var classAttribute = method.DeclaringType.GetCustomAttribute<RoutePrefixAttribute>();

            if (classAttribute == null && methodAttribute != null && !string.IsNullOrWhiteSpace(methodAttribute.Template))
                return methodAttribute.Template;

            if (classAttribute != null && methodAttribute != null && !string.IsNullOrWhiteSpace(methodAttribute.Template))
                return string.Format("{0}/{1}", classAttribute.Prefix, methodAttribute.Template);

            var parameters = method.GetParameters();
            var parameterString = parameters.Any(p => p.Name == "id")
                ? "/{id}"
                : "";

            if (classAttribute != null && methodAttribute == null)
                return string.Format("{0}{1}", classAttribute.Prefix, parameterString);

            if (method.DeclaringType == null)
                throw new NullReferenceException("DeclaringType can't be null");

            var typeName = method.DeclaringType.Name;

            return string.Format("{0}", typeName.Substring(0, typeName.IndexOf("Controller", StringComparison.Ordinal)).ToLower());

        }

        /// <summary>
        /// Gets the desired method from the ApiController based on HttpMethod and arguments
        /// 
        /// 1 - Check if there are methods:
        ///     a) case  0 methods : exception
        ///     b) case  1 method  : return this; todo: validate arguments
        ///     c) case >1 methods : step 2
        /// 
        /// 2 - Check if there are arguments and if these match on name (including templates)
        ///     a) case no arguments: this should be case 1b and not happening
        ///     b) case 1 match: return this
        ///     c) case >1 match: ask for explicit definition
        /// </summary>
        /// <param name="source"></param>
        /// <param name="method"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static MethodInfo GetAction(this Type source, HttpMethod method, IDictionary<string, object> arguments)
        {
            // Get the available actions from the controller type
            // todo: Validate the given type is indeed a controller
            var methods = source.GetMethods()
                .Where(p => p.IsPublic 
                    && !p.IsStatic 
                    && !p.IsSpecialName 
                    && !p.IsVirtual 
                    && !p.IsGenericMethod 
                    && !p.IsSecuritySafeCritical)
                .Select(methodInfo => new
                {
                    methodInfo,
                    httpMethod = methodInfo.GetHttpMethod(),
                    parameters = methodInfo.GetParameters()
                })
                .Where(p => p.httpMethod == method)
                .ToList();

            if (!methods.Any())
                throw new Exception(string.Format("No suitable action found for {0}", method));

            // If there's only one method on the controller, pass this one
            if (methods.Count() == 1)
                return methods.First().methodInfo;

            // Check if there are actions available with minimal or matching count of arguments with matching type
            // todo: Should be: Check if there are actions available with matching arguments NAMES

            var actionsWithEqualArguments = methods
                .Where(p => arguments.Count == p.parameters.Count()
                         && arguments.All(a => p.parameters.Any(r => r.Name == a.Key && r.ParameterType == a.Value.GetType())))
                .ToList();

            if (actionsWithEqualArguments.Count() > 1)
            {
                // multiple actions supporting this method try finding one without route template (=default action)
                var withoutRoute = actionsWithEqualArguments
                    .Where(p => p.methodInfo.GetCustomAttribute<RouteAttribute>() == null)
                    .ToList();

                if (withoutRoute.Count() > 1)
                    throw new Exception(string.Format("There are multiple actions supporting {0}, try specifying explicit", method));

                if (withoutRoute.Count() == 1)
                    return withoutRoute.Single().methodInfo;
            }

            return actionsWithEqualArguments.Any() 
                ? actionsWithEqualArguments.Single().methodInfo 
                : methods.First().methodInfo;
        }
    }
}