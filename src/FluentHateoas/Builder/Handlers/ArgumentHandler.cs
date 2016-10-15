using FluentHateoas.Builder.Factories;
using FluentHateoas.Handling;
using FluentHateoas.Interfaces;

namespace FluentHateoas.Builder.Handlers
{
    public class ArgumentHandler : RegistrationLinkHandlerBase
    {
        public override LinkBuilder Process<TModel>(IHateoasRegistration<TModel> definition, LinkBuilder resourceBuilder, TModel data)
        {
            resourceBuilder.Argument = definition.ArgumentDefinition.Compile()(data); // todo: Compile during registration?
            return base.Process(definition, resourceBuilder, data);
        }

        public override bool CanProcess<TModel>(IHateoasRegistration<TModel> definition, LinkBuilder resourceBuilder)
        {
            return definition.ArgumentDefinition != null;
        }
    }
}