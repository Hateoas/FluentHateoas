namespace FluentHateoas.Interfaces
{
    using System;
    using System.Linq.Expressions;

    public interface IGetExpressionBuilder<TModel> : 
        ITemplateExpressionBuilder<TModel>
    {
        // TODO Do not return HateoasExpressionBuilder<TModel>
        IWhenExpressionBuilder<TModel> When<TProvider>(Expression<Func<TProvider, TModel, bool>> when);
        ITemplateExpressionBuilder<TModel> AsCollection();
        IIdFromExpressionBuilder<TModel> IdFrom<TProvider>(Expression<Func<TProvider, TModel, object>> with);
    }
}