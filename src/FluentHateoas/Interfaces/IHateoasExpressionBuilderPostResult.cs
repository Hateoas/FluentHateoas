namespace FluentHateoas.Interfaces
{
    using System;
    using System.Linq.Expressions;

    using FluentHateoas.Registration;

    public interface IHateoasExpressionBuilderPostResult<TModel> : IHateoasExpressionBuilderBase<TModel>
    {
        // TODO Do not return HateoasExpressionBuilder<TModel>
        HateoasExpressionBuilder<TModel> WithCommand<TCommandFactory>();
        HateoasExpressionBuilder<TModel> WithCommand<TCommandFactory>(Expression<Func<TCommandFactory, object>> commandFactory);
    }
}