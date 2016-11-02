using FluentHateoas.Handling;
using FluentHateoas.Interfaces;

namespace FluentHateoas.Builder.Handlers
{
    public class ArgumentHandler : RegistrationLinkHandlerBase
    {
        private readonly IArgumentProcessor _idFromExpressionProcessor;
        private readonly IArgumentProcessor _argumentDefinitionsProcessor;
        private readonly IArgumentProcessor _templateArgumentsProcessor;

        public ArgumentHandler(
            IArgumentProcessor idFromExpressionProcessor,
            IArgumentProcessor argumentDefinitionsProcessor,
            IArgumentProcessor templateArgumentsProcessor)
        {
            _idFromExpressionProcessor = idFromExpressionProcessor;
            _argumentDefinitionsProcessor = argumentDefinitionsProcessor;
            _templateArgumentsProcessor = templateArgumentsProcessor;
        }

        /// <summary>
        /// Adds the arguments to the builder.
        /// 
        /// Remarkable at first sight might be adding the $$first$$
        /// 
        /// Example:
        /// Session.MenuId = 5;
        /// 
        /// container
        ///     .Register&lt;Session&gt;(p => p.MenuId)
        ///     .Use&lt;MenuController&gt;()
        /// 
        /// MenuId becomes Id in a later process
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="registration"></param>
        /// <param name="linkBuilder"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public override void ProcessInternal<TModel>(IHateoasRegistration<TModel> registration, ILinkBuilder linkBuilder, object data)
        {
            if (!_idFromExpressionProcessor.Process(registration, linkBuilder, data))
            {
                _argumentDefinitionsProcessor.Process(registration, linkBuilder, data);
            }

            _templateArgumentsProcessor.Process(registration, linkBuilder, data);
        }

        public override bool CanProcess<TModel>(IHateoasRegistration<TModel> registration, ILinkBuilder resourceBuilder)
        {
            return _argumentDefinitionsProcessor.CanProcess(registration, resourceBuilder) ||
                   _templateArgumentsProcessor.CanProcess(registration, resourceBuilder) ||
                   _idFromExpressionProcessor.CanProcess(registration, resourceBuilder);
        }
    }
}