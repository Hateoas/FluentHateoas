using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Http;
using FluentHateoas.Contracts;

namespace FluentHateoas.Registration
{
    public interface IHateoasExpression
    {
        Type Controller { get; }
        string Relation { get; }
        bool IsCollection { get; }
        LambdaExpression TargetAction { get; }
        HttpMethod HttpMethod { get; }
        bool Template { get; }
        bool Collection { get; }
        IEnumerable<LambdaExpression> TemplateParameters { get; }
        LambdaExpression WhenExpression { get; }
        LambdaExpression WithExpression { get; }
        Type Command { get; }
        LambdaExpression CommandFactory { get; }
    }

    public interface IHateoasExpression<TModel> : IHateoasExpression
    {
        Expression<Func<TModel, object>> IdentityDefinition { get; }
    }

    public class HateoasExpression : IHateoasExpression
    {
        protected HateoasExpression()
        {
        }

        public Type Controller { get; internal set; }

        public string Relation { get; internal set; }
        public bool IsCollection { get; internal set; }

        public LambdaExpression TargetAction { get; internal set; }
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
        private HateoasExpression() : base()
        {
        }

        public static HateoasExpression<TModel> Create(IHateoasRegistration<TModel> registration)
        {
            var expression = new HateoasExpression<TModel>
            {
                Relation = registration.Relation,
                IdentityDefinition = registration.IdentityDefinition,
                IsCollection = registration.IsCollection
            };

            return expression;
        }

        public Expression<Func<TModel, object>> IdentityDefinition { get; private set; }
    }
}