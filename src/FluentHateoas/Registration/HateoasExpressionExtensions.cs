﻿using System;
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

        private static void SetMethod<TController>(HttpMethod method, IHateoasExpression expression, LambdaExpression actionSelector)
        {
            expression.Controller = typeof(TController);
            expression.HttpMethod = method;
            expression.TargetAction = actionSelector;
        }
    }
}