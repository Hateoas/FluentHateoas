using FluentHateoas.Handling;
using FluentHateoas.Interfaces;

namespace FluentHateoas.Builder.Handlers
{
    public class RelationHandler : RegistrationLinkHandlerBase
    {
        protected override void ProcessInternal<TModel>(IHateoasRegistration<TModel> registration, LinkBuilder linkBuilder, object data)
        {
            linkBuilder.Relation = registration.Relation;
        }

        public override bool CanProcess<TModel>(IHateoasRegistration<TModel> registration, LinkBuilder linkBuilder)
        {
            return !string.IsNullOrWhiteSpace(registration.Relation);
        }
    }
}