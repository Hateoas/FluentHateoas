namespace FluentHateoas.Registration
{
    using System.Linq.Expressions;
    using System.Net.Http;
    using System.Web.Http.Controllers;

    public static class HateoasExpressionExtensions
    {
        public static void SetMethod<TController>(this HateoasExpression expression, HttpMethod method, LambdaExpression actionSelector) where TController : IHttpController
        {
            expression.Controller = typeof(TController);
            expression.HttpMethod = method;
            expression.TargetAction = actionSelector;
        }
    }
}