using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FluentHateoas.Helpers;

// ReSharper disable ArgumentsStyleNamedExpression

namespace FluentHateoas.Handling
{
    public class ConfigurationProviderGetLinksForFuncProvider : IConfigurationProviderGetLinksForFuncProvider
    {
        private readonly ICache<int, MethodInfo> _genericLinksForMethodsCache;

        public ConfigurationProviderGetLinksForFuncProvider(ICache<int, MethodInfo> genericLinksForMethodsCache)
        {
            _genericLinksForMethodsCache = genericLinksForMethodsCache;
        }

        public Func<IConfigurationProvider, object, IEnumerable<IHateoasLink>> GetLinksForFunc(IConfigurationProvider configurationProvider, Type contentType, object content)
        {
            Type singleContentType, contentTypeToUse;
            if (contentType.TryGetSingleItemType(out singleContentType))
                contentTypeToUse = singleContentType.MakeIEnumerableOfType();
            else
                singleContentType = contentTypeToUse = contentType;

            return GetLinksForFunc(
                configurationProvider,
                singleContentType,
                contentTypeToUse);
        }
        
        public Func<IConfigurationProvider, object, IEnumerable<IHateoasLink>> GetLinksForFunc(IConfigurationProvider configurationProvider, Type singleContentType, Type contentTypeToUse)
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

        private IEnumerable<MethodInfo> GetLinksForGenericMethods(IConfigurationProvider configurationProvider)
        {
            var cachedGenericMethods = _genericLinksForMethodsCache.Get();
            if (cachedGenericMethods.Any())
                return cachedGenericMethods;

            var configurationProviderType = configurationProvider.GetType();
            cachedGenericMethods = configurationProviderType
                .GetMethods()
                .Where(m => m.IsGenericMethod && m.Name == nameof(configurationProvider.GetLinksFor))
                .ToList();

            // ReSharper disable PossibleMultipleEnumeration
            foreach (var cachedGenericMethod in cachedGenericMethods)
                _genericLinksForMethodsCache.Add(cachedGenericMethod.GetHashCode(), cachedGenericMethod);

            return cachedGenericMethods;
            // ReSharper restore PossibleMultipleEnumeration
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