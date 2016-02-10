using System;
using System.Dynamic;
using System.Diagnostics.Contracts;
using FluentHateoas.Contracts;

namespace FluentHateoas.Registration
{
    public static class HateoasContainerExtensions
    {
        public static HateoasExpression<TModel> Register<TModel>(this IHateoasContainer container)
        {
            var registration = new HateoasRegistration<TModel>();
            container.Registrations.Add(registration);
            return HateoasExpressionFactory.Create<TModel>(registration);
        }

        public static void Configure<TModel>(this IHateoasContainer source, dynamic vars)
        {
            var container = source as HateoasContainer;

            if (container == null)
                throw new ArgumentException();

            container.Configuration.Extend(vars as ExpandoObject);
        }
    }
}