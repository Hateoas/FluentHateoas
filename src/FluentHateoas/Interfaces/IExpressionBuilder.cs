namespace FluentHateoas.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Web.Http.Controllers;

    public interface IExpressionBuilder<TModel> : IExpressionBuilderBase<TModel>
    {
        //IGetExpressionBuilder<TModel> Get<TController>(Expression<Func<TController, Action>> methodSelector) where TController : IHttpController;
        IGetExpressionBuilder<TModel> Get<TController>(Expression<Func<TController, Func<object>>> methodSelector) where TController : IHttpController;
        IGetExpressionBuilder<TModel> Get<TController>(Expression<Func<TController, Func<Guid, object>>> methodSelector) where TController : IHttpController;
        IGetExpressionBuilder<TModel> Get<TController>(Expression<Func<TController, Func<IEnumerable<TModel>>>> methodSelector) where TController : IHttpController;
        //IGetExpressionBuilder<TModel> Get<TController>(Expression<Func<TController, IEnumerable<TModel>>> methodSelector) where TController : IHttpController;
        IGetExpressionBuilder<TModel> Get<TController>() where TController : IHttpController;
        IPostExpressionBuilder<TModel> Post<TController>() where TController : IHttpController;
        IPostExpressionBuilder<TModel> Post<TController>(Expression<Func<TController, Action<TModel>>> methodSelector) where TController : IHttpController;
        IPostExpressionBuilder<TModel> Post<TController, TRequest, TResult>(Expression<Func<TController, Func<TRequest, TResult>>> methodSelector) where TController : IHttpController;
        IPutExpressionBuilder<TModel> Put<TController>() where TController : IHttpController;
        IDeleteExpressionBuilder<TModel> Delete<TController>() where TController : IHttpController;
        IDeleteExpressionBuilder<TModel> Delete<TController>(Expression<Func<TController, Action<TModel>>> methodSelector) where TController : IHttpController;
        IDeleteExpressionBuilder<TModel> Delete<TController>(Expression<Func<TController, Action<Guid>>> methodSelector) where TController : IHttpController;
    }
}