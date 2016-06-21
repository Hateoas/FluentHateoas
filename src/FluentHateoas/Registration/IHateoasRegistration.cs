namespace FluentHateoas.Registration
{
    using System;
    using System.Linq.Expressions;

    public interface IHateoasRegistration
    {
        string Relation { get; }
        bool IsCollection { get; }
    }

    public interface IHateoasRegistration<TModel> : IHateoasRegistration
    {
        Expression<Func<TModel, object>> IdentityDefinition { get; }
    }
}