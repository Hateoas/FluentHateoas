namespace FluentHateoas.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;

    public static class DynamicObjectHelper
    {
        public static ExpandoObject ToExpandoObject(dynamic dynamicObject)
        {
            if (dynamicObject == null)
            {
                return null;
            }

            var expandoObject = dynamicObject as ExpandoObject;
            if (expandoObject != null)
            {
                return expandoObject;
            }

            expandoObject = new ExpandoObject();
            var expandoDictionary = (IDictionary<string, object>)expandoObject;

            Type dynamicType = dynamicObject.GetType();
            var dynamicProperties = dynamicType.GetProperties();
            foreach (var property in dynamicProperties)
            {
                expandoDictionary[property.Name] = property.GetValue(dynamicObject);
            }

            return (ExpandoObject) expandoDictionary;
        }

        public static bool HasProperty(dynamic dynamicObject, string name)
        {
            if (dynamicObject == null)
            {
                return false;
            }

            Type objType = dynamicObject.GetType();

            if (objType == typeof(ExpandoObject))
            {
                return ((IDictionary<string, object>)dynamicObject).ContainsKey(name);
            }

            return objType.GetProperty(name) != null;
        }
    }
}