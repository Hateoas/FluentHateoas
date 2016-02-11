using System;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;

namespace FluentHateoas.Registration
{
    public static class HateoasExpressionExtensions
    {
        public static IHateoasExpression Get<TController>(this IHateoasExpression expression, LambdaExpression methodSelector = null)
        {
            SetMethod<TController>(HttpMethod.Get, expression, methodSelector);
            return expression;
        }

        public static IHateoasExpression Post<TController>(this IHateoasExpression expression, LambdaExpression methodSelector = null)
        {
            SetMethod<TController>(HttpMethod.Post, expression, methodSelector);
            return expression;
        }

        public static IHateoasExpression Put<TController>(this IHateoasExpression expression, LambdaExpression methodSelector = null)
        {
            SetMethod<TController>(HttpMethod.Put, expression, methodSelector);
            return expression;
        }

        public static IHateoasExpression Delete<TController>(this IHateoasExpression expression, LambdaExpression methodSelector = null)
        {
            SetMethod<TController>(HttpMethod.Delete, expression, methodSelector);
            return expression;
        }

        public static IHateoasExpression AsTemplate(this IHateoasExpression expression)
        {
            expression.Template = true;
            return expression;
        }

        public static IHateoasExpression AsCollection(this IHateoasExpression expression)
        {
            expression.Collection = true;
            return expression;
        }

        public static IHateoasExpression AsTemplate<TModel>(this IHateoasExpression expression, params Expression<Func<TModel, object>>[] args)
        {
            expression.Template = true;
            expression.TemplateParameters = args;
            return expression;
        }

        public static IHateoasExpression When<TProvider>(this IHateoasExpression expression, Expression<Func<TProvider, object, bool>> when)
        {
            expression.WhenExpression = when;
            return expression;
        }

        public static IHateoasExpression With<TProvider>(this IHateoasExpression expression, Expression<Func<TProvider, object, object>> with)
        {
            expression.WithExpression = with;
            return expression;
        }

        public static IHateoasExpression WithCommand<TCommand>(this IHateoasExpression expression)
        {
            expression.Command = typeof(TCommand);
            return expression;
        }

        public static IHateoasExpression WithCommand<TCommandFactory>(this IHateoasExpression expression, Expression<Func<TCommandFactory,object>> commandFactory)
        {
            expression.CommandFactory = commandFactory;
            return expression;
        }

        private static void SetMethod<TController>(HttpMethod method, IHateoasExpression expression, LambdaExpression actionSelector)
        {
            expression.Controller = typeof(TController);
            expression.HttpMethod = method;
            expression.TargetAction = actionSelector;
        }
    }
}