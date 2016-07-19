namespace FluentHateoas.Interfaces
{
    using System;
    using System.Linq.Expressions;

    using FluentHateoas.Registration;

    public interface ITemplateExpressionBuilder<TModel> : IExpressionBuilderBase<TModel>
    {
        ExpressionBuilder<TModel> AsTemplate();
        ExpressionBuilder<TModel> AsTemplate(params Expression<Func<TModel, object>>[] args);
    }
}