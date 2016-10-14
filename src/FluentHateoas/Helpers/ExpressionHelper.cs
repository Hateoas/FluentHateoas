using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

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

        public static MethodInfo GetMethodInfo<TController>(this Expression<Func<TController, Func<IEnumerable<object>>>> methodSelector)
        {
            var unaryExpression = (UnaryExpression)methodSelector.Body;
            var methodCallExpression = (MethodCallExpression)unaryExpression.Operand;
            var constantExpression = (ConstantExpression)methodCallExpression.Object;
            return (MethodInfo)constantExpression.Value;
        }
    }
}