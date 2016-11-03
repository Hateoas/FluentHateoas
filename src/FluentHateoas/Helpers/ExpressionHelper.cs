using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq.Expressions;
using System.Net.Http;
using System.Reflection;
using FluentHateoas.Builder.Model;

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
                var memberName = (
                    expression.Body as MemberExpression ??
                    (MemberExpression) ((UnaryExpression) expression.Body).Operand
                ).Member.Name;

                expandoDic.Add(memberName.Substring(0, 1).ToLowerInvariant() + memberName.Substring(1), expression.Compile().DynamicInvoke(data));
            }

            return expando;
        }

        public static MemberInfo GetMemberInfo<TModel>(this Expression<TModel> expression)
        {
            var expressionBody = expression?.Body as MemberExpression;

            if (expressionBody == null)
                throw new ArgumentException("expression is not a MemberExpression", nameof(expression));

            return expressionBody.Member;
        }

        /// <summary>
        /// This method returns the target action from a target action function.
        /// </summary>
        /// <param name="expression">The expression to extract the target action from.</param>
        /// <param name="relation"></param>
        /// <param name="httpMethod"></param>
        /// <param name="arguments"></param>
        /// <example>
        /// Expression should be something like:
        /// <code>Expression&lt;Func&lt;PersonController, Func&lt;object&gt;&gt;&gt; expression = p => p.Get;</code>
        ///  - or -
        /// <code>Expression&lt;Func&lt;PersonController, object&gt;&gt; expression = p => p.Get();</code>
        /// </example>
        /// <returns>The <see cref="MethodInfo">target action</see> for the given expression.</returns>
        public static MethodInfo GetTargetMethod(this LambdaExpression expression, string relation, HttpMethod httpMethod, IDictionary<string, Argument> arguments)
        {
            // e.g. Expression<Func<PersonController, Func<object>>> expression = p => p.Get;
            var unaryExpression = expression.Body as UnaryExpression;
            if (unaryExpression != null)
            {
                var methodCallExpression = unaryExpression.Operand as MethodCallExpression;
                if (methodCallExpression == null)
                    throw new InvalidOperationException("Expression is not of type MethodCallExpression");

                var constantExpression = methodCallExpression.Object as ConstantExpression;
                if (constantExpression == null)
                    throw new InvalidOperationException("Invalid ConstantExpression"); // todo: clarify this

                return (MethodInfo)constantExpression.Value;
            }

            // e.g. Expression<Func<PersonController, object>> expression = p => p.Get();
            var mce = expression.Body as MethodCallExpression;
            if (mce != null)
            {
                return mce.Method;
            }

            throw new NotImplementedException();
        }
    }
}