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

        public override void ProcessInternal<TModel>(IHateoasRegistration<TModel> definition, ILinkBuilder resourceBuilder, object data)
        {
            resourceBuilder.Success = resourceBuilder.Action != null && _authorizationProvider.IsAuthorized(resourceBuilder.Action);
        }

        public override bool CanProcess<TModel>(IHateoasRegistration<TModel> definition, ILinkBuilder resourceBuilder)
        {
            return true;
        }
    }
}