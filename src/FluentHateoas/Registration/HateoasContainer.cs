using System.Collections;
using System.Collections.Generic;
using FluentHateoas.Contracts;

namespace FluentHateoas.Registration
{
    public class HateoasContainer : IHateoasContainer
    {
        internal HateoasContainer(HateoasConfiguration configuration)
        {
            Configuration = configuration;
            Registrations = new List<IHateoasRegistration>();
        }

        public HateoasConfiguration Configuration { get; private set; }
        public IList<IHateoasRegistration> Registrations { get; private set; }
    }
}