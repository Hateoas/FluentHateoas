namespace FluentHateoas.Handling
{
    public interface IRegistrationLinkHandler
    {
        LinkBuilder Process<TModel>(Interfaces.IHateoasRegistration<TModel> registration, LinkBuilder linkBuilder, object data);
        void SetSuccessor(IRegistrationLinkHandler handler);
        bool CanProcess<TModel>(Interfaces.IHateoasRegistration<TModel> registration, LinkBuilder linkBuilder);
    }
}