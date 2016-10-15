using FluentHateoas.Handling;
using FluentHateoas.Interfaces;

namespace FluentHateoas.Builder.Handlers
{
    public class TemplateHandler : RegistrationLinkHandlerBase
    {
        public override LinkBuilder Process<TModel>(IHateoasRegistration<TModel> definition, LinkBuilder resourceBuilder, TModel data)
        {
            resourceBuilder.IsTemplate = definition.Expression.Template;

            return base.Process(definition, resourceBuilder, data);
        }

        public override bool CanProcess<TModel>(IHateoasRegistration<TModel> registration, LinkBuilder resourceBuilder)
        {
            return true;
        }
    }
}