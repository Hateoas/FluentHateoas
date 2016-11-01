namespace FluentHateoas.Handling
{
    public interface IRegistrationLinkHandler
    {
        ILinkBuilder Process<TModel>(Interfaces.IHateoasRegistration<TModel> registration, ILinkBuilder linkBuilder, object data);
        void SetSuccessor(IRegistrationLinkHandler handler);
        bool CanProcess<TModel>(Interfaces.IHateoasRegistration<TModel> registration, ILinkBuilder linkBuilder);
    }
}