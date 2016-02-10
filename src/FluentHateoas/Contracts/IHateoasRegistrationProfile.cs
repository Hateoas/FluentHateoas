namespace FluentHateoas.Contracts
{
    public interface IHateoasRegistrationProfile
    {
        void Register<TModel>(IHateoasContainer container);
    }
}