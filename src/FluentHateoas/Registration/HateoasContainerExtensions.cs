using System;
using System.Dynamic;
using FluentHateoas.Contracts;

namespace FluentHateoas.Registration
{
    public static class HateoasContainerExtensions
    {
        public static HateoasExpression Register(this IHateoasContainer container)
        {
            return HateoasExpressionFactory.Create(container);
        }

        public static void Configure(this IHateoasContainer source, dynamic vars)
        {
            var container = source as HateoasContainer;

            if (container == null)
                throw new ArgumentException();

            container.Configuration.Extend(vars as ExpandoObject); // TODO NULL
        }
    }
}