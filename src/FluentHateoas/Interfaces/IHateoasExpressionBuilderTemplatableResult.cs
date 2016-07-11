namespace FluentHateoas.Interfaces
{
    using System;
    using System.Linq.Expressions;

    using FluentHateoas.Registration;

    public interface IHateoasExpressionBuilderTemplatableResult<TModel> : IHateoasExpressionBuilderBase<TModel>
    {
        HateoasExpressionBuilder<TModel> AsTemplate();
        HateoasExpressionBuilder<TModel> AsTemplate(params Expression<Func<TModel, object>>[] args);
    }
}