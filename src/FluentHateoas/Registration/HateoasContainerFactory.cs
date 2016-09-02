namespace FluentHateoas.Registration
{
    using System.Web.Http;

    public static class HateoasContainerFactory
    {
        public static HateoasContainer Create(HttpConfiguration configuration)
        {
            return new HateoasContainer(configuration, new HateoasConfiguration());
        }
    }
}