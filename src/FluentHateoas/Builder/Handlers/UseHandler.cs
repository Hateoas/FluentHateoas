using FluentHateoas.Handling;
using FluentHateoas.Helpers;
using FluentHateoas.Interfaces;

namespace FluentHateoas.Builder.Handlers
{
    public class UseHandler : RegistrationLinkHandlerBase
    {
        public override LinkBuilder Process<TModel>(IHateoasRegistration<TModel> registration, LinkBuilder resourceBuilder, TModel data)
        {
            resourceBuilder.Controller = registration.Expression.Controller;
            resourceBuilder.Action = registration.Expression.GetTargetAction();

            return base.Process(registration, resourceBuilder, data);
        }

        public override bool CanProcess<TModel>(IHateoasRegistration<TModel> registration, LinkBuilder resourceBuilder)
        {
            return registration.Expression.Controller != null
                   && (registration.Expression.Action != null || registration.Expression.HttpMethod != null);
        }
    }
}