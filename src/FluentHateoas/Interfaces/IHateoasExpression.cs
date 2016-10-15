namespace FluentHateoas.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Net.Http;

    public interface IHateoasExpression
    {
        Type Controller { get; }
        string Relation { get; }
        bool IsCollection { get; }
        LambdaExpression Action { get; }
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
}