using System.Collections.Generic;
using FluentHateoas.Registration;

namespace FluentHateoas.Contracts
{
    public interface IHateoasContainer
    {
        HateoasConfiguration Configuration { get; }
        IList<IHateoasRegistration> Registrations { get; }
    }
}