namespace FluentHateoas.Handling
{
    public interface IRegistrationLinkHandler
    {
        LinkBuilder Process(Interfaces.IHateoasRegistration registration, LinkBuilder linkBuilder, object data);
        void SetSuccessor(IRegistrationLinkHandler handler);
        bool CanProcess(Interfaces.IHateoasRegistration registration, LinkBuilder linkBuilder);
    }
}