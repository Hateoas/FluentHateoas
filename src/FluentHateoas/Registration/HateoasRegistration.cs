using System;
using System.Linq.Expressions;

namespace FluentHateoas.Registration
{
    public class HateoasRegistration<TModel> : IHateoasRegistration<TModel>
    {
        public string Relation { get; }
        public Expression<Func<TModel, object>> IdentityDefinition { get; }
        public bool IsCollection { get; }

        public HateoasRegistration()
            : this("self", null, false)
        {
        }

        public HateoasRegistration(string relation)
            : this(relation, null, false)
        {
        }

        public HateoasRegistration(string relation, Expression<Func<TModel, object>> identityDefinition, bool isCollection = false)
        {
            Relation = relation;
            IdentityDefinition = identityDefinition;
            IsCollection = isCollection;
        }
    }
}