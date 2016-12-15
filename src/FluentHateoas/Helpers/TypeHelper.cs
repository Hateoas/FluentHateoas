using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentHateoas.Helpers
{
    public static class TypeHelper
    {
        public static bool TryGetSingleItemType(this Type potentialEnumerableType, out Type singleItemType)
        {
            if (potentialEnumerableType.IsOrImplementsIEnumerable())
            {
                singleItemType = potentialEnumerableType.IsArray
                    ? potentialEnumerableType.GetElementType()
                    : potentialEnumerableType.GetGenericArguments().Last();
                return true;
            }

            singleItemType = potentialEnumerableType;
            return false;
        }

        public static Type MakeIEnumerableOfType(this Type singleItemType)
        {
            //// The content type to use must be IEnumerable<TModel> in order to find the correct GetLinksFor method to use
            //// when generating the lambda function below (using SequenceEquals on types). In some cases, actual input will
            //// be List<TModel> or even TModel[]. For those cases, we just simplify the content type to its 'base form'.
            return typeof(IEnumerable<>).MakeGenericType(singleItemType);
        }
    }
}