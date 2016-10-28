using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace FluentHateoas.Handling
{
    public static class ConfigurationProviderExtensions
    {
        private static IList<MethodInfo> _genericLinksForMethods;

        public static Func<IConfigurationProvider, object, IEnumerable<IHateoasLink>> GetLinksForFunc(this IConfigurationProvider configurationProvider, Type contentType, object content)
        {
            Type singleContentType;
            if (contentType.GetInterfaces().Any(ti => ti.IsGenericType && ti.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
            {
                singleContentType = contentType.GetGenericArguments()[0];
                contentType = typeof(IEnumerable<>).MakeGenericType(singleContentType);
            }
            else
            {
                singleContentType = contentType;
            }

            var genericMethods = GetLinksForGenericMethods(configurationProvider);

            foreach (var genericMethod in genericMethods)
            {
                var method = genericMethod.MakeGenericMethod(singleContentType);

                var parameters = method.GetParameters().Select(p => p.ParameterType).ToArray();
                if (!parameters.SequenceEqual(new[] { contentType }))
                    continue;

                return CreateLinksForFunc(method, contentType);
            }

            return null;
        }

        private static IEnumerable<MethodInfo> GetLinksForGenericMethods(IConfigurationProvider configurationProvider)
        {
            if (_genericLinksForMethods != null)
                return _genericLinksForMethods;

            var configurationProviderType = configurationProvider.GetType();
            _genericLinksForMethods = configurationProviderType
                .GetMethods()
                .Where(m => m.IsGenericMethod && m.Name == nameof(configurationProvider.GetLinksFor))
                .ToList();
            return _genericLinksForMethods;
        }

        /// <summary>
        /// Get the GetLinksFor[TModel or IEnumerable[TModel]] function delegate corresponding
        /// to the given content type.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        private static Func<IConfigurationProvider, object, IEnumerable<IHateoasLink>> CreateLinksForFunc(MethodInfo method, Type contentType)
        {
            // TODO Maybe this functionality can move into 'genericity' even more, making it easier to reuse?
            var contentInputExpression = Expression.Parameter(typeof(object), "content");
            var convertedContentInputExpression = Expression.Convert(contentInputExpression, contentType);

            var instanceExpression = Expression.Parameter(typeof(IConfigurationProvider), "instance");
            var convertedInstanceExpression = method.DeclaringType != null
                ? (Expression) Expression.Convert(instanceExpression, method.DeclaringType)
                : instanceExpression;

            var methodCallExpr = Expression.Call(convertedInstanceExpression, method, convertedContentInputExpression);

            var result = Expression
                .Lambda<Func<IConfigurationProvider, object, IEnumerable<IHateoasLink>>>(
                    methodCallExpr, instanceExpression, contentInputExpression)
                .Compile();
            return result;
        }
    }
}