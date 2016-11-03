using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace FluentHateoas.Helpers
{
    public static class ObjectHelper
    {
        public static bool IsOrImplementsIEnumerable(this object source)
        {
            return IsOrImplementsIEnumerable(source.GetType());
        }


        public static object Materialize(this object source)
        {
            var sourceType = source.GetType();
            if (!IsOrImplementsIEnumerable(sourceType))
                return source;

            var genericTypeArgument = sourceType.GetGenericArguments().Last();
            var concreteListType = typeof(List<>).MakeGenericType(genericTypeArgument);
            if (sourceType == concreteListType)
                return source;

            // TODO: Quickfix for materialising WhereSelectListIterator and variants
            // Check if it is an iterator?
            var method = typeof(Enumerable)
                .GetMethod(nameof(Enumerable.ToList))
                .MakeGenericMethod(genericTypeArgument);

            return method.Invoke(null, new[] { source });
        }

        public static bool IsOrImplementsIEnumerable(Type contentType)
        {
            if (contentType.IsSimpleType())
                return false;

            if (contentType.IsInterface &&
                contentType.IsGenericType &&
                contentType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                return true;

            return
                contentType
                    .GetInterfaces()
                    .Any(ti => ti.IsGenericType && ti.GetGenericTypeDefinition() == typeof(IEnumerable<>));
        }

        private static bool IsSimpleType(this Type contentType)
        {
            return contentType.IsPrimitive
                   || contentType.IsEnum
                   || contentType == typeof(string)
                   || contentType == typeof(decimal);
        }
    }
}