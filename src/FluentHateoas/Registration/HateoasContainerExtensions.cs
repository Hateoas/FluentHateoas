using System;
using System.Collections;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using FluentHateoas.Contracts;

namespace FluentHateoas.Registration
{
    using FluentHateoas.Helpers;

    public static class HateoasContainerExtensions
    {
        public static HateoasExpressionBuilder<TModel> Register<TModel>(this IHateoasContainer container, string relation = null, Expression<Func<TModel, object>> identityDefinition = null)
        {
            if (typeof(TModel).GetInterfaces().Contains(typeof(IEnumerable)))
                throw new ArgumentException("Cannot register collections; use .AsCollection() instead");

            var registration = new HateoasRegistration<TModel>(relation, identityDefinition);
            container.Registrations.Add(registration);
            return HateoasExpressionFactory.CreateBuilder(registration);
        }

        public static void Configure(this IHateoasContainer source, dynamic vars)
        {
            var container = source as HateoasContainer;

            if (container == null)
                throw new ArgumentException();

            container.Configuration.Extend((ExpandoObject)DynamicObjectHelper.ToExpandoObject(vars));
        }
    }
}