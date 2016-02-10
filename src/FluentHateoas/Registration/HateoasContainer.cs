using FluentHateoas.Contracts;

namespace FluentHateoas.Registration
{
    public class HateoasContainer : IHateoasContainer
    {
        internal HateoasContainer(HateoasConfiguration configuration)
        {
            Configuration = configuration;
        }

        public HateoasConfiguration Configuration { get; private set; }
    }
}