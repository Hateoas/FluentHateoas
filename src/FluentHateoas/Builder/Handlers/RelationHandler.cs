using FluentHateoas.Handling;
using FluentHateoas.Interfaces;

namespace FluentHateoas.Builder.Handlers
{
    public class RelationHandler : RegistrationLinkHandlerBase
    {
        public override LinkBuilder Process<TModel>(IHateoasRegistration<TModel> registration, LinkBuilder linkBuilder, object data)
        {
            linkBuilder.Relation = registration.Relation;
            return base.Process(registration, linkBuilder, data);
        }

        public override bool CanProcess<TModel>(IHateoasRegistration<TModel> registration, LinkBuilder linkBuilder)
        {
            return !string.IsNullOrWhiteSpace(registration.Relation);
        }
    }
}