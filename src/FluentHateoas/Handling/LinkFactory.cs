using System.Linq;
using FluentHateoas.Builder.Handlers;
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
                yield return new RelationHandler();
                yield return new MethodHandler();
                yield return new CommandHandler();
                yield return new ArgumentHandler();
                yield return new TemplateHandler();
                yield return new UseHandler();
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

                linkBuilder.Success = linkBuilder.Controller != null && linkBuilder.Action != null;

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