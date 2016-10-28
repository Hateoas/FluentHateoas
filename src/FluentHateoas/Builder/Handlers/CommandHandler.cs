using FluentHateoas.Builder.Factories;
using FluentHateoas.Handling;
using FluentHateoas.Interfaces;

namespace FluentHateoas.Builder.Handlers
{
    public class CommandHandler : RegistrationLinkHandlerBase
    {
        protected override void ProcessInternal<TModel>(IHateoasRegistration<TModel> registration, LinkBuilder linkBuilder, object data)
        {
            linkBuilder.Command = registration.Expression.CreateCommand(registration.Relation + "-command");
        }

        public override bool CanProcess<TModel>(IHateoasRegistration<TModel> registration, LinkBuilder linkBuilder)
        {
            return registration.Expression.Command != null;
        }
    }
}