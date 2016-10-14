using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
                Type = expression.Command,
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
            return new Property
            {
                Type = propertyInfo.PropertyType.Name,
                Name = propertyInfo.Name,
                Order = index,
                //Required = propertyInfo
            };
        }
    }
}