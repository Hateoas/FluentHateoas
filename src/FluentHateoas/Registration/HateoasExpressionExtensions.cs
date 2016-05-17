using System;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;

namespace FluentHateoas.Registration
{
    public class HateoasExpressionBuilder<TModel>
    {
        private readonly HateoasExpression<TModel> _expression;

        public HateoasExpressionBuilder(HateoasRegistration<TModel> registration)
        {
            _expression = new HateoasExpression<TModel>(registration);
        }

        public HateoasExpressionBuilder<TModel> Get<TController>(LambdaExpression methodSelector = null)
        {
            SetMethod<TController>(HttpMethod.Get, methodSelector);
            return this;
        }

        public HateoasExpressionBuilder<TModel> Post<TController>(LambdaExpression methodSelector = null)
        {
            SetMethod<TController>(HttpMethod.Post, null);
            return this;
        }

        public HateoasExpressionBuilder<TModel> Put<TController>(LambdaExpression methodSelector = null)
        {
            SetMethod<TController>(HttpMethod.Put, methodSelector);
            return this;
        }

        public HateoasExpressionBuilder<TModel> Delete<TController>(LambdaExpression methodSelector = null)
        {
            SetMethod<TController>(HttpMethod.Delete, methodSelector);
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

        private void SetMethod<TController>(HttpMethod method, LambdaExpression actionSelector)
        {
            _expression.Controller = typeof(TController);
            _expression.HttpMethod = method;
            _expression.TargetAction = actionSelector;
        }
    }
}