using System.Net.Http;
using FluentHateoas.Handling;
using FluentHateoas.Helpers;
using FluentHateoas.Interfaces;

namespace FluentHateoas.Builder.Handlers
{
    public class UseHandler : RegistrationLinkHandlerBase
    {
        public override void ProcessInternal<TModel>(IHateoasRegistration<TModel> registration, ILinkBuilder linkBuilder, object data)
        {
            var expression = registration.Expression;
            linkBuilder.Controller = expression.Controller;
            linkBuilder.Action = expression.Action != null
                ? expression.Action.GetTargetMethod(linkBuilder.Relation, linkBuilder.Method, linkBuilder.Arguments)
                : expression.Controller.GetAction(linkBuilder.Relation, linkBuilder.Method ?? HttpMethod.Get, linkBuilder.Arguments);
        }

        public override bool CanProcess<TModel>(IHateoasRegistration<TModel> registration, ILinkBuilder resourceBuilder)
        {
            var expression = registration.Expression;
            return expression.Controller != null && (expression.Action != null || expression.HttpMethod != null);
        }
    }
}