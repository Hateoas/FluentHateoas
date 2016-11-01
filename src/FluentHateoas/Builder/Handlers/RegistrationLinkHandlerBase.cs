using System;
using FluentHateoas.Handling;
using FluentHateoas.Interfaces;

namespace FluentHateoas.Builder.Handlers
{
    public abstract class RegistrationLinkHandlerBase : IRegistrationLinkHandler
    {
        private IRegistrationLinkHandler _successor;

        public ILinkBuilder Process<TModel>(IHateoasRegistration<TModel> registration, ILinkBuilder resourceBuilder, object data)
        {
            if (CanProcess(registration, resourceBuilder))
            {
                ProcessInternal(registration, resourceBuilder, data);
            }

            return _successor != null
                ? _successor.Process(registration, resourceBuilder, data)
                : resourceBuilder;
        }

        public abstract void ProcessInternal<TModel>(IHateoasRegistration<TModel> registration, ILinkBuilder resourceBuilder, object data);

        public void SetSuccessor(IRegistrationLinkHandler handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));
            _successor = handler;
        }

        public abstract bool CanProcess<TModel>(IHateoasRegistration<TModel> registration, ILinkBuilder resourceBuilder);
    }
}