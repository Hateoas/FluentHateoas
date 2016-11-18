using System.Linq;
using System.Linq.Expressions;
using FluentHateoas.Builder.Model;
using FluentHateoas.Handling;
using FluentHateoas.Interfaces;

namespace FluentHateoas.Builder.Handlers
{
    public class ArgumentDefinitionsProcessor : ArgumentProcessor
    {
        public override bool CanProcess<TModel>(IHateoasRegistration<TModel> registration, ILinkBuilder linkBuilder)
        {
            return registration.ArgumentDefinitions != null && registration.ArgumentDefinitions.Any();
        }

        public override bool Process<TModel>(IHateoasRegistration<TModel> registration, ILinkBuilder linkBuilder, object data)
        {
            if (registration.ArgumentDefinitions == null) return false;

            var argumentDefinitionIndex = 0;
            foreach (var argumentDefinition in registration.ArgumentDefinitions)
            {
                // Add the first argument as 'id' so it always can be used as named property 'id'
                var keyOrigin = GetKey(data, ((MemberExpression)((UnaryExpression)argumentDefinition.Body).Operand).Member);
                var key = argumentDefinitionIndex == 0 ? "id" : keyOrigin;

                var value = argumentDefinition.Compile().DynamicInvoke(data);

                linkBuilder.Arguments.Add(key, new Argument { Name = key, Origin = keyOrigin, Type = value.GetType(), Value = value });
                argumentDefinitionIndex++;
            }

            return true;
        }

    }
}