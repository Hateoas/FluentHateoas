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

            // ToList on object
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