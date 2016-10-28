namespace FluentHateoas.Contracts
{
    using FluentHateoas.Interfaces;

    public interface IHateoasContainer
    {
        IHateoasConfiguration Configuration { get; }
        //IList<IHateoasRegistration> Registrations { get; }

        void Add(IHateoasRegistration registration);
        void Update();
        void Update(IHateoasRegistration registration);
    }
}