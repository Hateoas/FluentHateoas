using FluentHateoas.Handling;
using FluentHateoas.Interfaces;

namespace FluentHateoas.Builder.Handlers
{
    public class TemplateHandler : RegistrationLinkHandlerBase
    {
        public override void ProcessInternal<TModel>(IHateoasRegistration<TModel> definition, LinkBuilder resourceBuilder, object data)
        {
            resourceBuilder.IsTemplate = definition.Expression.Template;
        }

        public override bool CanProcess<TModel>(IHateoasRegistration<TModel> registration, LinkBuilder resourceBuilder)
        {
            return true;
        }
    }
}