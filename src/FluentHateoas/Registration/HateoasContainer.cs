using FluentHateoas.Contracts;

namespace FluentHateoas.Registration
{
    public class HateoasContainer : IHateoasContainer
    {
        public HateoasConfiguration Configuration { get; set; }
    }
}