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
                //yield return new InitHandler();
                //yield return new UseHandler();
                //yield return new WhenNotNullHandler();
                //yield return new FixedRouteHandler();
                //yield return new WithHandler();
                //yield return new TemplateHandler();
                return null;
            }
        }

        public System.Collections.Generic.IEnumerable<IHateoasLink> CreateLinks(System.Collections.Generic.List<Interfaces.IHateoasRegistration> registrations, object data)
        {
            return Enumerable.Select(
                Enumerable.Where(
                    Enumerable.Select(registrations, definition => 
                    _handlerChain.Process(definition, new LinkBuilder(), data)), linkBuilder => linkBuilder.Success), linkBuilder => LinkBuilderExtensions.Build(linkBuilder));
        }
    }
}