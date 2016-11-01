using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentHateoas.Helpers
{
    internal static class ObjectHelper
    {
        internal static bool IsOrImplementsIEnumerable(this object source)
        {
            return IsOrImplementsIEnumerable(source.GetType());
        }


        internal static object Materialize(this object source)
        {
            if (!source.IsOrImplementsIEnumerable())
                return source;

            // todo: Quikfix for materialising WehereSelectList and variants
            // Check if it is an iterator?
            var method = typeof(Enumerable)
                .GetMethod(nameof(Enumerable.ToList))
                .MakeGenericMethod(source.GetType().GetGenericArguments().Last());

            return method.Invoke(null, new[] { source });
        }

        internal static bool IsOrImplementsIEnumerable(Type contentType)
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
    }
}