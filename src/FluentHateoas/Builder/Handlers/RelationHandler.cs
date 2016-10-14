using FluentHateoas.Handling;
using FluentHateoas.Interfaces;

namespace FluentHateoas.Builder.Handlers
{
    public class RelationHandler : RegistrationLinkHandlerBase
    {
        public override LinkBuilder Process(IHateoasRegistration registration, LinkBuilder linkBuilder, object data)
        {
            linkBuilder.Relation = registration.Relation;
            return base.Process(registration, linkBuilder, data);
        }

        public override bool CanProcess(IHateoasRegistration registration, LinkBuilder linkBuilder)
        {
            return !string.IsNullOrWhiteSpace(registration.Relation);
        }
    }
}