using FluentHateoas.Contracts;

namespace FluentHateoas.Registration
{
    public class HateoasExpression<TModel>
    {
        private readonly HateoasRegistration<TModel> _registration;

        public HateoasExpression(HateoasRegistration<TModel> registration)
        {
            _registration = registration;
        }
    }
}