using System;
using FluentHateoas.Interfaces;

namespace FluentHateoas.Handling.Handlers
{
    public abstract class RegistrationLinkHandlerBase : IRegistrationLinkHandler
    {
        private IRegistrationLinkHandler _successor;

        public virtual LinkBuilder Process(IHateoasRegistration definition, LinkBuilder resourceBuilder, object data)
        {
            return _successor != null && _successor.CanProcess(definition, resourceBuilder)
                ? _successor.Process(definition, resourceBuilder, data)
                : resourceBuilder;
        }

        public void SetSuccessor(IRegistrationLinkHandler handler)
        {
            if (handler == null) throw new ArgumentNullException("handler");

            _successor = handler;
        }

        public abstract bool CanProcess(IHateoasRegistration definition, LinkBuilder resourceBuilder);
    }
}