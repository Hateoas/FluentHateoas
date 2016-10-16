using FluentHateoas.Handling;
using FluentHateoas.Interfaces;

namespace FluentHateoas.Builder.Handlers
{
    public class SuccessHandler : RegistrationLinkHandlerBase
    {
        public SuccessHandler()
        {
            
        }

        public override LinkBuilder Process<TModel>(IHateoasRegistration<TModel> definition, LinkBuilder resourceBuilder, TModel data)
        {
            return base.Process(definition, resourceBuilder, data);
        }

        public override bool CanProcess<TModel>(IHateoasRegistration<TModel> definition, LinkBuilder resourceBuilder)
        {
            return true;
        }
    }
}