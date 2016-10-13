using System.Linq;
using FluentHateoas.Handling.Handlers;
using Enumerable = System.Linq.Enumerable;

namespace FluentHateoas.Handling
{
    public class LinkFactory : ILinkFactory
    {
        private readonly IRegistrationLinkHandler _handlerChain;

        public LinkFactory(params IRegistrationLinkHandler[] handlers)
        {
            _handlerChain = (handlers.Length > 0 ? handlers : DefaultHandlers).CreateChain();
        }

        public System.Collections.Generic.IEnumerable<IRegistrationLinkHandler> DefaultHandlers
        {
            get
            {
                yield return new RelationNameHandler();
                //yield return new UseHandler();
                //yield return new WhenNotNullHandler();
                //yield return new FixedRouteHandler();
                //yield return new WithHandler();
                //yield return new TemplateHandler();
            }
        }

        public System.Collections.Generic.IEnumerable<IHateoasLink> CreateLinks(System.Collections.Generic.List<Interfaces.IHateoasRegistration> registrations, object data)
        {
            return registrations.Select(definition => _handlerChain
                    .Process(definition, new LinkBuilder(), data))
                    .Where(linkBuilder => linkBuilder.Success)
                    .Select(linkBuilder => linkBuilder.Build());
        }
    }
}