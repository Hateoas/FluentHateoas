namespace FluentHateoas.Interfaces
{
    using System;
    using System.Linq.Expressions;

    using FluentHateoas.Registration;

    public interface IGetExpressionBuilder<TModel> : 
        ITemplateExpressionBuilder<TModel>
    {
        // TODO Do not return HateoasExpressionBuilder<TModel>
        IWhenExpressionBuilder<TModel> When<TProvider>(Expression<Func<TProvider, TModel, bool>> when);
        ITemplateExpressionBuilder<TModel> AsCollection();
    }
}