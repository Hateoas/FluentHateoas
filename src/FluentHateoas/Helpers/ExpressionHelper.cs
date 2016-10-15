using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using FluentHateoas.Interfaces;

namespace FluentHateoas.Helpers
{
    public static class ExpressionHelper
    {
        public static MemberInfo GetMemberInfo<TModel>(this Expression<TModel> expression)
        {
            var expressionBody = expression.Body as MemberExpression;

            if (expressionBody == null)
                throw new ArgumentException("expression is not a MemberExpression");

            return expressionBody.Member;
        }

        public static MethodInfo GetTargetAction(this LambdaExpression expression)
        {
            var unaryExpression = (UnaryExpression)expression.Body;
            var methodCallExpression = (MethodCallExpression)unaryExpression.Operand;
            var constantExpression = (ConstantExpression)methodCallExpression.Object;

            if (constantExpression == null)
                throw new Exception("Invalid ConstantExpression"); // todo: clearify this

            return (MethodInfo)constantExpression.Value;
        }

        public static MethodInfo GetTargetAction<TModel>(this IHateoasExpression<TModel> source)
        {
            if (source.Action != null)
                return source.Action.GetTargetAction();

            throw new NotImplementedException();
        }
    }
}