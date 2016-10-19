using FluentHateoas.Handling;
using FluentHateoas.Interfaces;
using FluentHateoas.Registration;

namespace FluentHateoas.Builder.Handlers
{
    public class SuccessHandler : RegistrationLinkHandlerBase
    {
        private readonly IAuthorizationProvider _authorizationProvider;

        public SuccessHandler(IAuthorizationProvider authorizationProvider)
        {
            _authorizationProvider = authorizationProvider;
        }

        public override LinkBuilder Process<TModel>(IHateoasRegistration<TModel> definition, LinkBuilder resourceBuilder, object data)
        {
            resourceBuilder.Success = resourceBuilder.Action != null && _authorizationProvider.IsAuthorized(resourceBuilder.Action);
            return base.Process(definition, resourceBuilder, data);
        }

        public override bool CanProcess<TModel>(IHateoasRegistration<TModel> definition, LinkBuilder resourceBuilder)
        {
            return true;
        }
    }
}