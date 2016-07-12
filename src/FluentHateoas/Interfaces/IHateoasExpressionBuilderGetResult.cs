namespace FluentHateoas.Interfaces
{
    using System;
    using System.Linq.Expressions;

    using FluentHateoas.Registration;

    public interface IHateoasExpressionBuilderGetResult<TModel> : 
        IHateoasExpressionBuilderTemplatableResult<TModel>,
        IHateoasExpressionBuilderBase<TModel>
    {
        // TODO Do not return HateoasExpressionBuilder<TModel>
        HateoasExpressionBuilder<TModel> When<TProvider>(Expression<Func<TProvider, TModel, bool>> when);
        IHateoasExpressionBuilderTemplatableResult<TModel> AsCollection();
    }
}