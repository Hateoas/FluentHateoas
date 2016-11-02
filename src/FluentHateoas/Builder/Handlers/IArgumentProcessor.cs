using FluentHateoas.Handling;
using FluentHateoas.Interfaces;

namespace FluentHateoas.Builder.Handlers
{
    public interface IArgumentProcessor
    {
        bool CanProcess<TModel>(IHateoasRegistration<TModel> registration, ILinkBuilder linkBuilder);
        bool Process<TModel>(IHateoasRegistration<TModel> registration, ILinkBuilder linkBuilder, object data);
    }
}