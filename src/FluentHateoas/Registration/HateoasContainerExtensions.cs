using System;
using System.Collections;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using FluentHateoas.Contracts;
using FluentHateoas.Helpers;
using FluentHateoas.Interfaces;

namespace FluentHateoas.Registration
{
    public static class HateoasContainerExtensions
    {
        public static IExpressionBuilder<TModel> Register<TModel>(this IHateoasContainer container, string relation, params Expression<Func<TModel, object>>[] identityDefinition)
        {
            if (typeof(TModel).GetInterfaces().Contains(typeof(IEnumerable)))
                throw new ArgumentException("Cannot register collections; use .RegisterCollection<TModel>(\"name\") instead");

            // TODO The relation between (container,) registration and expression builder feels weird 
            var registration = new HateoasRegistration<TModel>(relation, identityDefinition, container);
            var builder = HateoasExpressionFactory.CreateBuilder(registration);
            container.Add(registration);
            return builder;
        }

        public static IExpressionBuilder<TModel> Register<TModel>(this IHateoasContainer container, Expression<Func<TModel, object>> relation, params Expression<Func<TModel, object>>[] identityDefinition)
        {
            if (typeof(TModel).GetInterfaces().Contains(typeof(IEnumerable)))
                throw new ArgumentException("Cannot register collections; use .RegisterCollection<TModel>(\"name\") instead");

            var registration = new HateoasRegistration<TModel>(((MemberExpression) relation.Body).Member.Name, identityDefinition, container, false, true);
            var builder = HateoasExpressionFactory.CreateBuilder(registration);
            container.Add(registration);
            return builder;
        }

        public static IExpressionBuilder<TModel> RegisterCollection<TModel>(this IHateoasContainer container, string relation, params Expression<Func<TModel, object>>[] identityDefinition)
        {
            var registration = new HateoasRegistration<TModel>(relation, null, container, true);
            var builder = HateoasExpressionFactory.CreateBuilder(registration);
            container.Add(registration);
            return builder;
        }

        public static IExpressionBuilder<TModel> RegisterCollection<TModel>(this IHateoasContainer container, Expression<Func<TModel, object>> relation, params Expression<Func<TModel, object>>[] identityDefinition)
        {
            var registration = new HateoasRegistration<TModel>(((MemberExpression)relation.Body).Member.Name, null, container, true, true);
            var builder = HateoasExpressionFactory.CreateBuilder(registration);
            container.Add(registration);
            return builder;
        }

        public static void Configure(this IHateoasContainer source, dynamic vars)
        {
            var container = source as HateoasContainer;

            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            container.Configuration.Extend((ExpandoObject)DynamicObjectHelper.ToExpandoObject(vars));
            container.Update();
        }
    }
}
