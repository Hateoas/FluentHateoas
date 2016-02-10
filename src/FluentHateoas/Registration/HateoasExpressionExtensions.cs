using System;
using System.Linq.Expressions;
using System.Net.Http;

namespace FluentHateoas.Registration
{
    public static class HateoasExpressionExtensions
    {
        public static HateoasExpression<TModel> Get<TController, TModel>(this HateoasExpression<TModel> expression, string relation = null, LambdaExpression methodSelector = null)
        {
            SetMethod<TController, TModel>(HttpMethod.Get, expression, relation, methodSelector);
            return expression;
        }

        public static HateoasExpression<TModel> Post<TController, TModel>(this HateoasExpression<TModel> expression, string relation = null, LambdaExpression methodSelector = null)
        {
            SetMethod<TController, TModel>(HttpMethod.Post, expression, relation, methodSelector);
            return expression;
        }

        public static HateoasExpression<TModel> Put<TController, TModel>(this HateoasExpression<TModel> expression, string relation = null, LambdaExpression methodSelector = null)
        {
            SetMethod<TController, TModel>(HttpMethod.Put, expression, relation, methodSelector);
            return expression;
        }

        public static HateoasExpression<TModel> Delete<TController, TModel>(this HateoasExpression<TModel> expression, string relation = null, LambdaExpression methodSelector = null)
        {
            SetMethod<TController, TModel>(HttpMethod.Delete, expression, relation, methodSelector);
            return expression;
        }

        private static void SetMethod<TController, TModel>(HttpMethod get, HateoasExpression<TModel> expression, string relation, LambdaExpression actionSelector)
        {
            expression.Controller = typeof(TController);
            expression.Relation = relation ?? "self";
            expression.TargetAction = actionSelector;
        }
    }
}