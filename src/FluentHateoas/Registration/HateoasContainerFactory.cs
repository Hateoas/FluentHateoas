namespace FluentHateoas.Registration
{
    public static class HateoasContainerFactory
    {
        public static HateoasContainer Create()
        {
            return new HateoasContainer(new HateoasConfiguration());
        }
    }
}