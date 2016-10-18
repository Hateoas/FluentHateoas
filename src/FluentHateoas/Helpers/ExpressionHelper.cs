using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq.Expressions;
using System.Net.Http;
using System.Reflection;
using FluentHateoas.Interfaces;

namespace FluentHateoas.Helpers
{
    public static class ExpressionHelper
    {
        public static ExpandoObject ToExpando<TModel>(this Expression<Func<TModel, object>>[] source, TModel data)
        {
            var expando = new ExpandoObject();
            var expandoDic = (IDictionary<string, object>)expando;

            foreach (var expression in source)
            {
                var id = ((MemberExpression) ((UnaryExpression) expression.Body).Operand).Member.Name;

                expandoDic.Add(id.Substring(0, 1).ToLowerInvariant() + id.Substring(1), expression.Compile().DynamicInvoke(data));
            }

            return expando;
        }

        public static MemberInfo GetMemberInfo<TModel>(this Expression<TModel> expression)
        {
            var expressionBody = expression.Body as MemberExpression;

            if (expressionBody == null)
                throw new ArgumentException("expression is not a MemberExpression");

            return expressionBody.Member;
        }

        public static MethodInfo GetTargetAction(this LambdaExpression expression, string relation, HttpMethod httpMethod, IDictionary<string, object> arguments)
        {
            var unaryExpression = expression.Body as UnaryExpression;
            if (unaryExpression != null)
            {
                var methodCallExpression = (MethodCallExpression) unaryExpression.Operand;
                var constantExpression = (ConstantExpression) methodCallExpression.Object;

                if (constantExpression == null)
                    throw new Exception("Invalid ConstantExpression"); // todo: clearify this

                return (MethodInfo) constantExpression.Value;
            }
            else
            {
                //var methodCallExpression = (MethodCallExpression)expression.Body;
                //var constantExpression = (ConstantExpression)methodCallExpression.Object;

                //if (constantExpression == null)
                //    throw new Exception("Invalid ConstantExpression"); // todo: clearify this

                //return (MethodInfo)constantExpression.Value;
                throw new NotImplementedException();
            }
        }
    }
}