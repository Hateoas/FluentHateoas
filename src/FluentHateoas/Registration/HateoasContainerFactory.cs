using FluentHateoas.Handling;

namespace FluentHateoas.Registration
{
    public static class HateoasContainerFactory
    {
        public static HateoasContainer Create(IHttpConfiguration configuration)
        {
            return new HateoasContainer(configuration, new HateoasConfiguration());
        }
    }
}