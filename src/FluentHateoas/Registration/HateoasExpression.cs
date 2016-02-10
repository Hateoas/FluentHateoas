using System;
using System.Linq.Expressions;
using FluentHateoas.Contracts;

namespace FluentHateoas.Registration
{
    public class HateoasExpression<TModel>
    {
        private readonly HateoasRegistration<TModel> _registration;

        public HateoasExpression(HateoasRegistration<TModel> registration)
        {
            _registration = registration;
        }

        public Type Controller { get; set; }
        public string Relation { get; set; }
        public LambdaExpression TargetAction { get; set; }
    }
}