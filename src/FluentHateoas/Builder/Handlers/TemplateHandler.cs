using FluentHateoas.Handling;
using FluentHateoas.Interfaces;

namespace FluentHateoas.Builder.Handlers
{
    public class TemplateHandler : RegistrationLinkHandlerBase
    {
        public override void ProcessInternal<TModel>(IHateoasRegistration<TModel> definition, ILinkBuilder resourceBuilder, object data)
        {
            resourceBuilder.IsTemplate = definition.Expression.Template;
        }

        public override bool CanProcess<TModel>(IHateoasRegistration<TModel> registration, ILinkBuilder resourceBuilder)
        {
            return true;
        }
    }
}