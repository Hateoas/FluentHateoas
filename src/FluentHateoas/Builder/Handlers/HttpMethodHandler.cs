using FluentHateoas.Handling;
using FluentHateoas.Interfaces;

namespace FluentHateoas.Builder.Handlers
{
    public class HttpMethodHandler : RegistrationLinkHandlerBase
    {
        public override void ProcessInternal<TModel>(IHateoasRegistration<TModel> definition, LinkBuilder resourceBuilder, object data)
        {
            resourceBuilder.Method = definition.Expression.HttpMethod;
        }

        public override bool CanProcess<TModel>(IHateoasRegistration<TModel> definition, LinkBuilder resourceBuilder)
        {
            return true;
        }
    }
}