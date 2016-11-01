using FluentHateoas.Handling;
using FluentHateoas.Interfaces;

namespace FluentHateoas.Builder.Handlers
{
    public class HttpMethodHandler : RegistrationLinkHandlerBase
    {
        public override void ProcessInternal<TModel>(IHateoasRegistration<TModel> definition, ILinkBuilder linkBuilder, object data)
        {
            linkBuilder.Method = definition.Expression.HttpMethod;
        }

        public override bool CanProcess<TModel>(IHateoasRegistration<TModel> definition, ILinkBuilder resourceBuilder)
        {
            return true;
        }
    }
}