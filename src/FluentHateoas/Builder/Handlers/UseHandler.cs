using System;
using System.Net.Http;
using FluentHateoas.Handling;
using FluentHateoas.Helpers;
using FluentHateoas.Interfaces;

namespace FluentHateoas.Builder.Handlers
{
    public class UseHandler : RegistrationLinkHandlerBase
    {
        public override LinkBuilder Process<TModel>(IHateoasRegistration<TModel> registration, LinkBuilder resourceBuilder, object data)
        {
            resourceBuilder.Controller = registration.Expression.Controller;

            if (registration.Expression.Action != null)
                resourceBuilder.Action = registration.Expression.Action.GetTargetAction(resourceBuilder.Relation, resourceBuilder.Method, resourceBuilder.Arguments);

            else if (registration.Expression.Action == null)
                resourceBuilder.Action = registration.Expression.Controller.GetAction(resourceBuilder.Relation, resourceBuilder.Method ?? HttpMethod.Get, resourceBuilder.Arguments);

            else
                throw new NotImplementedException();


            return base.Process(registration, resourceBuilder, data);
        }

        public override bool CanProcess<TModel>(IHateoasRegistration<TModel> registration, LinkBuilder resourceBuilder)
        {
            return registration.Expression.Controller != null
                   && (registration.Expression.Action != null || registration.Expression.HttpMethod != null);
        }
    }
}