namespace FluentHateoas.Interfaces
{
    using System;
    using System.Linq.Expressions;

    public interface IWithCommandExpressionBuilder<TModel> : IExpressionBuilderBase<TModel>
    {
    }

    public interface IPostExpressionBuilder<TModel> : IExpressionBuilderBase<TModel>
    {
        // TODO Do not return HateoasExpressionBuilder<TModel>
        IWithCommandExpressionBuilder<TModel> WithCommand<TCommandFactory>();
        IWithCommandExpressionBuilder<TModel> WithCommand<TCommandFactory>(Expression<Func<TCommandFactory, object>> commandFactory);
        IIdFromExpressionBuilder<TModel> IdFrom<TProvider>(Expression<Func<TProvider, TModel, object>> with);
    }
}