using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Http;

namespace FluentHateoas.Registration
{
    using FluentHateoas.Interfaces;

    public abstract class HateoasExpression : IHateoasExpression
    {
        public Type Controller { get; internal set; }

        public string Relation { get; internal set; }
        public bool IsCollection { get; internal set; }

        public LambdaExpression Action { get; internal set; }
        public HttpMethod HttpMethod { get; internal set; }
        public bool Template { get; internal set; }
        public bool Collection { get; internal set; }
        public IEnumerable<LambdaExpression> TemplateParameters { get; internal set; }
        public LambdaExpression WhenExpression { get; internal set; }
        public LambdaExpression WithExpression { get; internal set; }
        public Type Command { get; internal set; }
        public LambdaExpression CommandFactory { get; internal set; }
    }

    public sealed class HateoasExpression<TModel> : HateoasExpression, IHateoasExpression<TModel>
    {
        private HateoasExpression()
        {
        }

        internal static HateoasExpression<TModel> Create(IHateoasRegistration<TModel> registration)
        {
            var expression = new HateoasExpression<TModel>
            {
                Relation = registration.Relation,
                ArgumentDefinitions = registration.ArgumentDefinitions,
                IsCollection = registration.IsCollection
            };

            return expression;
        }

        public Expression<Func<TModel, object>>[] ArgumentDefinitions { get; private set; }
    }
}