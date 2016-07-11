namespace FluentHateoas.Contracts
{
    using System.Collections.Generic;

    using FluentHateoas.Interfaces;
    using FluentHateoas.Registration;

    public interface IHateoasContainer
    {
        HateoasConfiguration Configuration { get; }
        IList<IHateoasRegistration> Registrations { get; }
    }
}