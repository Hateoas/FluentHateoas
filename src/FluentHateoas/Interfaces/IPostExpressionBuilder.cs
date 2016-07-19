namespace FluentHateoas.Interfaces
{
    using System;
    using System.Linq.Expressions;

    using FluentHateoas.Registration;

    public interface IPostExpressionBuilder<TModel> : IExpressionBuilderBase<TModel>
    {
        // TODO Do not return HateoasExpressionBuilder<TModel>
        ExpressionBuilder<TModel> WithCommand<TCommandFactory>();
        ExpressionBuilder<TModel> WithCommand<TCommandFactory>(Expression<Func<TCommandFactory, object>> commandFactory);
    }
}