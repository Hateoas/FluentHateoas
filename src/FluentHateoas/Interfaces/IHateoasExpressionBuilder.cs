namespace FluentHateoas.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Web.Http.Controllers;

    public interface IHateoasExpressionBuilder<TModel> : IHateoasExpressionBuilderBase<TModel>
    {
        IHateoasExpressionBuilderGetResult<TModel> Get<TController>(Expression<Func<TController, Func<IEnumerable<TModel>>>> methodSelector) where TController : IHttpController;
        IHateoasExpressionBuilderGetResult<TModel> Get<TController>(Expression<Func<TController, IEnumerable<TModel>>> methodSelector) where TController : IHttpController;
        IHateoasExpressionBuilderGetResult<TModel> Get<TController>(Expression<Func<TController, Func<Guid, TModel>>> methodSelector) where TController : IHttpController;
        IHateoasExpressionBuilderGetResult<TModel> Get<TController>(LambdaExpression methodSelector = null) where TController : IHttpController;
        IHateoasExpressionBuilderPostResult<TModel> Post<TController>(LambdaExpression methodSelector = null) where TController : IHttpController;
        IHateoasExpressionBuilderPutResult<TModel> Put<TController>(LambdaExpression methodSelector = null) where TController : IHttpController;
        IHateoasExpressionBuilderDeleteResult<TModel> Delete<TController>(LambdaExpression methodSelector = null) where TController : IHttpController;
    }
}