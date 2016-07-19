namespace FluentHateoas.Interfaces
{
    using System;
    using System.Linq.Expressions;

    public interface IWithExpressionBuilder<TModel> : ITemplateExpressionBuilder<TModel>
    {
    }

    public interface IWhenExpressionBuilder<TModel> : ITemplateExpressionBuilder<TModel>
    {
        IWithExpressionBuilder<TModel> With<TProvider>(Expression<Func<TProvider, TModel, object>> with);
    }

    public interface ITemplateExpressionBuilder<TModel> : IExpressionBuilderBase<TModel>
    {
        IExpressionBuilderBase<TModel> AsTemplate();
        IExpressionBuilderBase<TModel> AsTemplate(params Expression<Func<TModel, object>>[] args);
    }
}