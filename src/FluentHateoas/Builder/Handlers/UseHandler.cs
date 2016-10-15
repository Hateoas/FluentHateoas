using FluentHateoas.Handling;
using FluentHateoas.Interfaces;

namespace FluentHateoas.Builder.Handlers
{
    public class UseHandler : RegistrationLinkHandlerBase
    {
        public override LinkBuilder Process<TModel>(IHateoasRegistration<TModel> definition, LinkBuilder resourceBuilder, TModel data)
        {
            return base.Process(definition, resourceBuilder, data);
        }

        public override bool CanProcess<TModel>(IHateoasRegistration<TModel> definition, LinkBuilder resourceBuilder)
        {
            return definition.Expression.Controller != null
                   && (definition.Expression.TargetAction != null || definition.Expression.HttpMethod != null);
        }
    }
}