using System;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;

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

            var typeName = method.DeclaringType.Name;
            return string.Format("{0}", typeName.Substring(0, typeName.IndexOf("Controller")).ToLower());

        }

        public static MethodInfo GetAction(this Type source, HttpMethod method)
        {
            var methods = source.GetMethods()
                .Where(p => p.IsPublic && !p.IsStatic && !p.IsSpecialName && !p.IsVirtual && !p.IsGenericMethod && !p.IsSecuritySafeCritical)
                .Select(methodInfo => new { methodInfo, httpMethod = methodInfo.GetHttpMethod() })
                .Where(p => p.httpMethod == method)
                .ToList();

            if (!methods.Any())
                throw new Exception(string.Format("No suitable action found for {0}", method));

            if (methods.Count() > 1)
                throw new Exception(string.Format("There are multiple actions supporting {0}, try specifying explicit", method));

            return methods.Single().methodInfo;
        }
    }
}