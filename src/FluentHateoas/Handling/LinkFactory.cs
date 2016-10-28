using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;
using FluentHateoas.Builder.Handlers;
using FluentHateoas.Registration;

namespace FluentHateoas.Handling
{
    public class LinkFactory : ILinkFactory
    {
        private readonly IAuthorizationProvider _authorizationProvider;
        private readonly IDependencyResolver _dependencyResolver;
        private readonly IRegistrationLinkHandler _handlerChain;

        public LinkFactory(IAuthorizationProvider authorizationProvider, IDependencyResolver dependencyResolver, params IRegistrationLinkHandler[] handlers)
        {
            _authorizationProvider = authorizationProvider;
            _dependencyResolver = dependencyResolver;
            _handlerChain = (handlers.Length > 0 ? handlers : DefaultHandlers).CreateChain();
        }

        public IEnumerable<IRegistrationLinkHandler> DefaultHandlers
        {
            get
            {
                yield return new RelationHandler();
                yield return new HttpMethodHandler();
                yield return new CommandHandler();
                yield return new ArgumentHandler(_dependencyResolver);
                yield return new TemplateHandler();
                yield return new UseHandler();
                yield return new SuccessHandler(_authorizationProvider);
                // yield return new WhenNotNullHandler();
                // yield return new FixedRouteHandler();
                // yield return new WithHandler();
            }
        }

        public IEnumerable<IHateoasLink> CreateLinks<TModel>(IEnumerable<Interfaces.IHateoasRegistration<TModel>> registrations, object data)
        {
            return registrations.Select(registration =>
            {
                var linkBuilder = new LinkBuilder(data);
                _handlerChain.Process(registration, linkBuilder, data);
                return linkBuilder;
            })
            .Where(linkBuilder => linkBuilder.Success)
            .Select(linkBuilder => linkBuilder.Build());
        }
    }
}