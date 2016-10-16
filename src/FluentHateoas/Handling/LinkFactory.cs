using System.Linq;
using System.Web.Http.Dependencies;
using FluentHateoas.Builder.Handlers;
using FluentHateoas.Handling.Handlers;
using FluentHateoas.Registration;
using Enumerable = System.Linq.Enumerable;

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

        public System.Collections.Generic.IEnumerable<IRegistrationLinkHandler> DefaultHandlers
        {
            get
            {
                yield return new RelationHandler();
                yield return new MethodHandler();
                yield return new CommandHandler();
                yield return new ArgumentHandler(_dependencyResolver);
                yield return new TemplateHandler();
                yield return new UseHandler();
                yield return new SuccessHandler(_authorizationProvider);
                //yield return new WhenNotNullHandler();
                //yield return new FixedRouteHandler();
                //yield return new WithHandler();
            }
        }

        public System.Collections.Generic.IEnumerable<IHateoasLink> CreateLinks<TModel>(System.Collections.Generic.List<Interfaces.IHateoasRegistration<TModel>> registrations, TModel data)
        {
            var handlers = DefaultHandlers.ToList();
            return registrations.Select(p =>
            {
                var linkBuilder = new LinkBuilder(data);
                handlers.ForEach(h =>
                {
                    if (h.CanProcess(p, linkBuilder))
                        h.Process(p, linkBuilder, data);
                });

                return linkBuilder;
            })
            .Where(linkBuilder => linkBuilder.Success)
            .Select(linkBuilder => linkBuilder.Build());
            //return registrations.Select(definition => _handlerChain
            //        .Process(definition, new LinkBuilder(), data))
            //        .Where(linkBuilder => linkBuilder.Success)
            //        .Select(linkBuilder => linkBuilder.Build());
        }
    }
}