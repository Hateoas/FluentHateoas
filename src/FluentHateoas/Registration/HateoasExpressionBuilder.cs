using System;
using System.Linq.Expressions;
using System.Net.Http;

namespace FluentHateoas.Registration
{
    using System.Collections.Generic;
    using System.Web.Http.Controllers;

    public class HateoasExpressionBuilder<TModel>
    {
        private readonly HateoasExpression<TModel> _expression;

        public HateoasExpressionBuilder(HateoasRegistration<TModel> registration)
        {
            _expression = HateoasExpression<TModel>.Create(registration);
        }

        public HateoasExpressionBuilder<TModel> Get<TController>(Expression<Func<TController, Func<IEnumerable<TModel>>>> methodSelector) where TController : IHttpController
        {
            _expression.SetMethod<TController>(HttpMethod.Get, methodSelector);
            return this;
        }

        public HateoasExpressionBuilder<TModel> Get<TController>(Expression<Func<TController, IEnumerable<TModel>>> methodSelector) where TController : IHttpController
        {
            _expression.SetMethod<TController>(HttpMethod.Get, methodSelector);
            return this;
        }

        public HateoasExpressionBuilder<TModel> Get<TController>(Expression<Func<TController, Func<Guid, TModel>>> methodSelector) where TController : IHttpController
        {
            _expression.SetMethod<TController>(HttpMethod.Get, methodSelector);
            return this;
        }

        public HateoasExpressionBuilder<TModel> Get<TController>(LambdaExpression methodSelector = null) where TController : IHttpController
        {
            _expression.SetMethod<TController>(HttpMethod.Get, methodSelector);
            return this;
        }

        public HateoasExpressionBuilder<TModel> Post<TController>(LambdaExpression methodSelector = null) where TController : IHttpController
        {
            _expression.SetMethod<TController>(HttpMethod.Post, null);
            return this;
        }

        public HateoasExpressionBuilder<TModel> Put<TController>(LambdaExpression methodSelector = null) where TController : IHttpController
        {
            _expression.SetMethod<TController>(HttpMethod.Put, methodSelector);
            return this;
        }

        public HateoasExpressionBuilder<TModel> Delete<TController>(LambdaExpression methodSelector = null) where TController : IHttpController
        {
            _expression.SetMethod<TController>(HttpMethod.Delete, methodSelector);
            return this;
        }

        public HateoasExpressionBuilder<TModel> AsTemplate()
        {
            _expression.Template = true;
            return this;
        }

        public HateoasExpressionBuilder<TModel> AsCollection()
        {
            _expression.Collection = true;
            return this;
        }

        public HateoasExpressionBuilder<TModel> AsTemplate(params Expression<Func<TModel, object>>[] args)
        {
            _expression.Template = true;
            _expression.TemplateParameters = args;
            return this;
        }

        public HateoasExpressionBuilder<TModel> When<TProvider>(Expression<Func<TProvider, object, bool>> when)
        {
            _expression.WhenExpression = when;
            return this;
        }

        public HateoasExpressionBuilder<TModel> With<TProvider>(Expression<Func<TProvider, object, object>> with)
        {
            _expression.WithExpression = with;
            return this;
        }

        public HateoasExpressionBuilder<TModel> WithCommand<TCommand>()
        {
            _expression.Command = typeof(TCommand);
            return this;
        }

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