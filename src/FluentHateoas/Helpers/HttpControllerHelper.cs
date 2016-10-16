using System;
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

        public static MethodInfo GetAction(this Type source, HttpMethod method, params object[] args)
        {
            var methods = source.GetMethods()
                .Where(p => p.IsPublic && !p.IsStatic && !p.IsSpecialName && !p.IsVirtual && !p.IsGenericMethod && !p.IsSecuritySafeCritical)
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

            if (methods.Count() == 1)
                return methods.First().methodInfo;

            var results = methods
                .Where(m => string.Join(",", m.parameters.Select(p => p.ParameterType.FullName)) == string.Join(",", args.Select(a => (a ?? new object()).GetType().FullName))) // todo: new object => hack, should be clearer
                .ToList();

            if (results.Count() > 1)
            {
                // multiple actions supporting this method try finding one without route template (=default action)
                var withoutRoute = results
                    .Where(p => p.methodInfo.GetCustomAttribute<RouteAttribute>() == null)
                    .ToList();

                if (withoutRoute.Count() > 1)
                    throw new Exception(string.Format("There are multiple actions supporting {0}, try specifying explicit", method));

                if (withoutRoute.Count() == 1)
                    return withoutRoute.Single().methodInfo;
            }

            return results.Any() 
                ? results.Single().methodInfo 
                : methods.First().methodInfo;
        }
    }
}