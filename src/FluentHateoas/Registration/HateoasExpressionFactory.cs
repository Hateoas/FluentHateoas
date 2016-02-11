using System.Collections.Generic;
using FluentHateoas.Contracts;

namespace FluentHateoas.Registration
{
    public static class HateoasExpressionFactory
    {
        public static HateoasExpression<TModel> Create<TModel>(HateoasRegistration<TModel> registration)
        {
            return new HateoasExpression<TModel>(registration);
        }
    }
}