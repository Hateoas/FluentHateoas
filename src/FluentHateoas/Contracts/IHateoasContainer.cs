using System.Collections.Generic;
using FluentHateoas.Registration;

namespace FluentHateoas.Contracts
{
    public interface IHateoasContainer
    {
        IList<IHateoasRegistration> Registrations { get; }
    }
}