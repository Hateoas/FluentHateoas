using System;
using System.Dynamic;
using FluentHateoas.Contracts;

namespace FluentHateoas.Registration
{
    public static class HateoasContainerExtensions
    {
        public static HateoasExpression<TModel> Register<TModel>(this IHateoasContainer container, string relation = null)
        {
            var registration = new HateoasRegistration<TModel>(relation);
            container.Registrations.Add(registration);
            return HateoasExpressionFactory.Create<TModel>(registration);
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