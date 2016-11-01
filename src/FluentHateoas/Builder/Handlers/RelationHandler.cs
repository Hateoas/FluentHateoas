using FluentHateoas.Handling;
using FluentHateoas.Interfaces;

namespace FluentHateoas.Builder.Handlers
{
    public class RelationHandler : RegistrationLinkHandlerBase
    {
        public override void ProcessInternal<TModel>(IHateoasRegistration<TModel> registration, ILinkBuilder linkBuilder, object data)
        {
            linkBuilder.Relation = registration.Relation;
        }

        public override bool CanProcess<TModel>(IHateoasRegistration<TModel> registration, ILinkBuilder linkBuilder)
        {
            return !string.IsNullOrWhiteSpace(registration.Relation);
        }
    }
}