namespace FluentHateoas.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;

    public static class DynamicObjectHelper
    {
        public static ExpandoObject ToExpandoObject(dynamic dynamicObject)
        {
            var expandoObject = new ExpandoObject();
            var expandoDictionary = (IDictionary<string, object>)expandoObject;

            Type dynamicType = dynamicObject.GetType();
            var dynamicProperties = dynamicType.GetProperties();
            foreach (var property in dynamicProperties)
            {
                expandoDictionary[property.Name] = property.GetValue(dynamicObject);
            }

            return expandoObject;
        }

        public static bool HasProperty(dynamic obj, string name)
        {
            Type objType = obj.GetType();

            if (objType == typeof(ExpandoObject))
            {
                return ((IDictionary<string, object>)obj).ContainsKey(name);
            }

            return objType.GetProperty(name) != null;
        }
    }
}