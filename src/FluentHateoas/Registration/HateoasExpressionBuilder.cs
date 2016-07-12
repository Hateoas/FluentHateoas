// ReSharper disable UnusedMethodReturnValue.Global
// ReSharper disable UnusedParameter.Global

namespace FluentHateoas.Registration
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Net.Http;
    using System.Web.Http.Controllers;

    using FluentHateoas.Interfaces;

    public class HateoasExpressionBuilder<TModel> : 
        IHateoasExpressionBuilder<TModel>,
        IHateoasExpressionBuilderGetResult<TModel>,
        IHateoasExpressionBuilderPostResult<TModel>,
        IHateoasExpressionBuilderPutResult<TModel>,
        IHateoasExpressionBuilderDeleteResult<TModel>
    {
        private readonly HateoasExpression<TModel> _expression;

        public HateoasExpressionBuilder(HateoasRegistration<TModel> registration)
        {
            _expression = HateoasExpression<TModel>.Create(registration);
        }

        public IHateoasExpressionBuilderGetResult<TModel> Get<TController>(Expression<Func<TController, Func<IEnumerable<TModel>>>> methodSelector) where TController : IHttpController
        {
            _expression.SetMethod<TController>(HttpMethod.Get, methodSelector);
            return this;
        }

        public IHateoasExpressionBuilderGetResult<TModel> Get<TController>(Expression<Func<TController, IEnumerable<TModel>>> methodSelector) where TController : IHttpController
        {
            _expression.SetMethod<TController>(HttpMethod.Get, methodSelector);
            return this;
        }

        public IHateoasExpressionBuilderGetResult<TModel> Get<TController>(Expression<Func<TController, Func<Guid, TModel>>> methodSelector) where TController : IHttpController
        {
            _expression.SetMethod<TController>(HttpMethod.Get, methodSelector);
            return this;
        }

        public IHateoasExpressionBuilderGetResult<TModel> Get<TController>(LambdaExpression methodSelector = null) where TController : IHttpController
        {
            _expression.SetMethod<TController>(HttpMethod.Get, methodSelector);
            return this;
        }

        public IHateoasExpressionBuilderPostResult<TModel> Post<TController>(LambdaExpression methodSelector = null) where TController : IHttpController
        {
            _expression.SetMethod<TController>(HttpMethod.Post, null);
            return this;
        }

        public IHateoasExpressionBuilderPutResult<TModel> Put<TController>(LambdaExpression methodSelector = null) where TController : IHttpController
        {
            _expression.SetMethod<TController>(HttpMethod.Put, methodSelector);
            return this;
        }

        public IHateoasExpressionBuilderDeleteResult<TModel> Delete<TController>(LambdaExpression methodSelector = null) where TController : IHttpController
        {
            _expression.SetMethod<TController>(HttpMethod.Delete, methodSelector);
            return this;
        }

        public IHateoasExpressionBuilderTemplatableResult<TModel> AsCollection()
        {
            _expression.Collection = true;
            return this;
        }

        HateoasExpressionBuilder<TModel> IHateoasExpressionBuilderTemplatableResult<TModel>.AsTemplate()
        {
            _expression.Template = true;
            return this;
        }

        HateoasExpressionBuilder<TModel> IHateoasExpressionBuilderTemplatableResult<TModel>.AsTemplate(params Expression<Func<TModel, object>>[] args)
        {
            _expression.Template = true;
            _expression.TemplateParameters = args;
            return this;
        }

        // TODO BL Add TProvider constraint(s)
        public HateoasExpressionBuilder<TModel> When<TProvider>(Expression<Func<TProvider, TModel, bool>> when)
        {
            _expression.WhenExpression = when;
            return this;
        }

        // TODO BL Add TProvider constraint(s)
        public HateoasExpressionBuilder<TModel> With<TProvider>(Expression<Func<TProvider, TModel, object>> with)
        {
            _expression.WithExpression = with;
            return this;
        }

        // TODO BL Add TCommand constraint(s)
        public HateoasExpressionBuilder<TModel> WithCommand<TCommand>()
        {
            _expression.Command = typeof(TCommand);
            return this;
        }

        // TODO BL Add TCommandFactory constraint(s)
        public HateoasExpressionBuilder<TModel> WithCommand<TCommandFactory>(Expression<Func<TCommandFactory, object>> commandFactory)
        {
            _expression.CommandFactory = commandFactory;
            return this;
        }

        public IHateoasExpression<TModel> GetExpression()
        {
            return _expression;
        }
    }
}