﻿namespace FluentHateoas.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Web.Http.Controllers;

    public interface IExpressionBuilder<TModel> : IExpressionBuilderBase<TModel>
    {
        IGetExpressionBuilder<TModel> Get<TController>(Expression<Func<TController, Func<IEnumerable<TModel>>>> methodSelector) where TController : IHttpController;
        IGetExpressionBuilder<TModel> Get<TController>(Expression<Func<TController, IEnumerable<TModel>>> methodSelector) where TController : IHttpController;
        IGetExpressionBuilder<TModel> Get<TController>(Expression<Func<TController, Func<Guid, TModel>>> methodSelector) where TController : IHttpController;
        IGetExpressionBuilder<TModel> Get<TController>(LambdaExpression methodSelector = null) where TController : IHttpController;
        IPostExpressionBuilder<TModel> Post<TController>(LambdaExpression methodSelector = null) where TController : IHttpController;
        IPutExpressionBuilder<TModel> Put<TController>(LambdaExpression methodSelector = null) where TController : IHttpController;
        IDeleteExpressionBuilder<TModel> Delete<TController>(LambdaExpression methodSelector = null) where TController : IHttpController;
    }
}