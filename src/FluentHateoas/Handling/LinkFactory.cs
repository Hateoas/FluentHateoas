using System.Collections.Generic;
using System.Linq;
using FluentHateoas.Builder.Handlers;
using FluentHateoas.Registration;

namespace FluentHateoas.Handling
{
    public class LinkFactory : ILinkFactory
    {
        private readonly IAuthorizationProvider _authorizationProvider;
        private readonly IRegistrationLinkHandler _handlerChain;

        private readonly IArgumentProcessor _idFromExpressionProcessor;
        private readonly IArgumentProcessor _argumentsDefinitionsProcessor;
        private readonly IArgumentProcessor _templateArgumentsProcessor;

        public LinkFactory(
            IAuthorizationProvider authorizationProvider,
            IArgumentProcessor idFromExpressionProcessor,
            IArgumentProcessor argumentsDefinitionsProcessor,
            IArgumentProcessor templateArgumentsProcessor,
            params IRegistrationLinkHandler[] handlers)
        {
            _authorizationProvider = authorizationProvider;
            _idFromExpressionProcessor = idFromExpressionProcessor;
            _argumentsDefinitionsProcessor = argumentsDefinitionsProcessor;
            _templateArgumentsProcessor = templateArgumentsProcessor;
            _handlerChain = (handlers.Length > 0 ? handlers : DefaultHandlers).CreateChain();
        }

        public IEnumerable<IRegistrationLinkHandler> DefaultHandlers
        {
            get
            {
                yield return new RelationHandler();
                yield return new HttpMethodHandler();
                yield return new CommandHandler();
                yield return new ArgumentHandler(
                    _idFromExpressionProcessor,
                    _argumentsDefinitionsProcessor,
                    _templateArgumentsProcessor);
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