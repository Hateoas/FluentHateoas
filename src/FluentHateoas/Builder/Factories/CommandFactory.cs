using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentHateoas.Attributes;
using FluentHateoas.Builder.Model;
using FluentHateoas.Interfaces;

namespace FluentHateoas.Builder.Factories
{
    public static class CommandFactory
    {
        public static Command CreateCommand(this IHateoasExpression expression, string name)
        {
            return new Command
            {
                Name = name,
                Type = expression.Command.Name,
                Properties = expression.Command.AsCommandProperties().ToList()
            };
        }

        public static IEnumerable<Property> AsCommandProperties(this Type source)
        {
            if (source == null)
                return new List<Property>();

            return source.GetProperties().Select(CreateProperty);
        }

        private static Property CreateProperty(PropertyInfo propertyInfo, int index)
        {
            var nullable = propertyInfo.PropertyType.IsGenericType && propertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>);
            var type = nullable
                ? Nullable.GetUnderlyingType(propertyInfo.PropertyType)
                : propertyInfo.PropertyType;

            switch (type.Name)
            {
                case "Int32":
                case "Int64":
                    return CreateIntProperty(propertyInfo, index, nullable);

                default:
                    return CreateProperty<Property>(propertyInfo, index, nullable);
            }
        }

        private static Property CreateIntProperty(PropertyInfo propertyInfo, int index, bool nullable)
        {
            var result = CreateProperty<IntegerProperty>(propertyInfo, index, nullable);

            var minValue = propertyInfo.GetCustomAttribute<MinValueAttribute>();
            var maxValue = propertyInfo.GetCustomAttribute<MaxValueAttribute>();

            if (minValue != null)
            {
                result.Min = minValue.MinimumValue;
            }

            if (maxValue != null)
            {
                result.Max = maxValue.MaximumValue;
            }
            return result;
        }

        private static TProperty CreateProperty<TProperty>(PropertyInfo propertyInfo, int index, bool nullable) where TProperty : Property, new()
        {
            return new TProperty
            {
                Type = propertyInfo.PropertyType.Name,
                Name = propertyInfo.Name,
                Order = index,
                Required = !nullable
            };
        }
    }
}