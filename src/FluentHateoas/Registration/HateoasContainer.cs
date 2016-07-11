namespace FluentHateoas.Registration
{
    using System.Collections.Generic;
    using FluentHateoas.Contracts;
    using FluentHateoas.Interfaces;

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