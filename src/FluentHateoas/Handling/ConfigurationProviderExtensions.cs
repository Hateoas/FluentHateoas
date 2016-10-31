using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

// ReSharper disable ArgumentsStyleNamedExpression

namespace FluentHateoas.Handling
{
    public static class ConfigurationProviderExtensions
    {
        private static IEnumerable<MethodInfo> _genericLinksForMethods;

        public static Func<IConfigurationProvider, object, IEnumerable<IHateoasLink>> GetLinksForFunc(this IConfigurationProvider configurationProvider, Type contentType, object content)
        {
            var determinedContentTypes = DetermineContentTypes(contentType);
            return GetLinksForFunc(
                configurationProvider,
                singleContentType: determinedContentTypes.Item1,
                contentTypeToUse: determinedContentTypes.Item2);
        }

        private static Tuple<Type, Type> DetermineContentTypes(Type contentType)
        {
            Type singleContentType, contentTypeToUse;

            if(IsOrImplementsIEnumerable(contentType))
            {
                singleContentType = contentType.GetGenericArguments()[0];
                // The content type to use must be IEnumerable<TModel> in order to find the correct GetLinksFor method to use
                // when generating the lambda function below (using SequenceEquals on types). In some cases, actual input will
                // be List<TModel> or even TModel[]. For those cases, we just simplify the content type to its 'base form'.
                contentTypeToUse = typeof(IEnumerable<>).MakeGenericType(singleContentType);
            }
            else
            {
                singleContentType = contentType;
                contentTypeToUse = contentType;
            }

            return new Tuple<Type, Type>(singleContentType, contentTypeToUse);
        }

        private static bool IsOrImplementsIEnumerable(Type contentType)
        {
            if (contentType.IsInterface && 
                contentType.IsGenericType &&
                contentType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                return true;

            return
                contentType
                    .GetInterfaces()
                    .Any(ti => ti.IsGenericType && ti.GetGenericTypeDefinition() == typeof(IEnumerable<>));
        }

        private static Func<IConfigurationProvider, object, IEnumerable<IHateoasLink>> GetLinksForFunc(IConfigurationProvider configurationProvider, Type singleContentType, Type contentTypeToUse)
        {
            var genericMethods = GetLinksForGenericMethods(configurationProvider);

            foreach (var genericMethod in genericMethods)
            {
                var method = genericMethod.MakeGenericMethod(singleContentType);

                var parameters = method.GetParameters().Select(p => p.ParameterType).ToArray();
                if (!parameters.SequenceEqual(new[] { contentTypeToUse }))
                    continue;

                return CreateLinksForFunc(method, contentTypeToUse);
            }

            throw new InvalidOperationException($"No HATEOAS configuration for type {contentTypeToUse.Name}");
        }

        private static IEnumerable<MethodInfo> GetLinksForGenericMethods(IConfigurationProvider configurationProvider)
        {
            if (_genericLinksForMethods != null)
                return _genericLinksForMethods;

            var configurationProviderType = configurationProvider.GetType();
            _genericLinksForMethods = configurationProviderType
                .GetMethods()
                .Where(m => m.IsGenericMethod && m.Name == nameof(configurationProvider.GetLinksFor));
            return _genericLinksForMethods;
        }

        /// <summary>
        /// Get the GetLinksFor[TModel or IEnumerable[TModel]] function delegate corresponding to the given content type.
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
                ? (Expression)Expression.Convert(instanceExpression, method.DeclaringType)
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