namespace FluentHateoas.Interfaces
{
    using System;
    using System.Linq.Expressions;

    public interface IHateoasRegistration
    {
        Type Model { get; }
        string Relation { get; }
        bool IsCollection { get; }
        IHateoasExpression Expression { get; set; }
    }

    public interface IHateoasRegistration<TModel> : IHateoasRegistration
    {
        Expression<Func<TModel, object>>[] ArgumentDefinitions { get; }
        new IHateoasExpression<TModel> Expression { get; set; }
    }
}