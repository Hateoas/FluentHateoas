namespace FluentHateoas.Interfaces
{
    using System;
    using System.Linq.Expressions;

    using FluentHateoas.Registration;

    public interface IPutExpressionBuilder<TModel> : IExpressionBuilderBase<TModel>
    {
        // TODO Do not return HateoasExpressionBuilder<TModel>
        IWithCommandExpressionBuilder<TModel> WithCommand<TCommandFactory>();
        IWithCommandExpressionBuilder<TModel> WithCommand<TCommandFactory>(Expression<Func<TCommandFactory, object>> commandFactory);
    }
}