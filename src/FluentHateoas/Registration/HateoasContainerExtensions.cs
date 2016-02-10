using FluentHateoas.Contracts;

namespace FluentHateoas.Registration
{
    public static class HateoasContainerExtensions
    {
        public static HateoasExpression Register(this IHateoasContainer container)
        {
            return new HateoasExpression();
        }
    }
}