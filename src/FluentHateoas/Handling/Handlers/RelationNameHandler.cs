using FluentHateoas.Interfaces;

namespace FluentHateoas.Handling.Handlers
{
    public class RelationNameHandler : RegistrationLinkHandlerBase
    {
        public override LinkBuilder Process(IHateoasRegistration registration, LinkBuilder linkBuilder, object data)
        {
            linkBuilder.Relation = registration.Relation;
            return base.Process(registration, linkBuilder, data);
        }

        public override bool CanProcess(IHateoasRegistration registration, LinkBuilder linkBuilder)
        {
            return registration.Relation != null;
        }
    }
}