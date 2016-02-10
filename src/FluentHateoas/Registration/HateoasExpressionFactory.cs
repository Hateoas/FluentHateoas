using FluentHateoas.Contracts;

namespace FluentHateoas.Registration
{
    public static class HateoasExpressionFactory
    {
        public static HateoasExpression Create(IHateoasContainer container)
        {
            return new HateoasExpression(container);
        }
    }
}