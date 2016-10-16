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
            switch (propertyInfo.PropertyType.Name)
            {
                case "Int32":
                case "Int64":
                    return CreateIntProperty(propertyInfo, index);

                default:
                    return CreateProperty<Property>(propertyInfo, index);
            }
        }

        private static Property CreateIntProperty(PropertyInfo propertyInfo, int index)
        {
            var result = CreateProperty<IntegerProperty>(propertyInfo, index);

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

        private static TProperty CreateProperty<TProperty>(PropertyInfo propertyInfo, int index) where TProperty : Property, new()
        {
            return new TProperty
            {
                Type = propertyInfo.PropertyType.Name,
                Name = propertyInfo.Name,
                Order = index,
                //Required = propertyInfo
            };
        }
    }
}