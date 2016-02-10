using FluentHateoas.Contracts;

namespace FluentHateoas.Registration
{
    public class HateoasExpression
    {
        private readonly IHateoasContainer _hateoasContainer;

        public HateoasExpression(IHateoasContainer hateoasContainer)
        {
            _hateoasContainer = hateoasContainer;
        }
    }
}