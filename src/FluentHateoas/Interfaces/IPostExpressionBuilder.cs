namespace FluentHateoas.Interfaces
{
    using System;
    using System.Linq.Expressions;

    using FluentHateoas.Registration;

    public interface IWithCommandExpressionBuilder<TModel> : IExpressionBuilderBase<TModel>
    {
    }

    public interface IPostExpressionBuilder<TModel> : IExpressionBuilderBase<TModel>
    {
        // TODO Do not return HateoasExpressionBuilder<TModel>
        IWithCommandExpressionBuilder<TModel> WithCommand<TCommandFactory>();
        IWithCommandExpressionBuilder<TModel> WithCommand<TCommandFactory>(Expression<Func<TCommandFactory, object>> commandFactory);
    }
}