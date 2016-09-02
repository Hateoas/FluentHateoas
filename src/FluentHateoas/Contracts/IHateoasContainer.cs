using System;

namespace FluentHateoas.Contracts
{
    using System.Collections.Generic;

    using FluentHateoas.Interfaces;
    using FluentHateoas.Registration;

    public interface IHateoasContainer
    {
        IHateoasConfiguration Configuration { get; }
        //IList<IHateoasRegistration> Registrations { get; }

        void Add(IHateoasRegistration registration);
        void Update();
        void Update(IHateoasRegistration registration);
    }
}