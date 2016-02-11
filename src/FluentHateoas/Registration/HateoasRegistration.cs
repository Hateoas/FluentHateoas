using System;
using System.Linq.Expressions;

namespace FluentHateoas.Registration
{
    public class HateoasRegistration<TModel> : IHateoasRegistration
    {
        private readonly string _relation;
        private readonly Expression<Func<TModel, object>> _expression;
        private readonly bool _isCollection;

        public HateoasRegistration() : this("self")
        { }

        public HateoasRegistration(string relation) : this (relation, null)
        { }

        public HateoasRegistration(string relation, Expression<Func<TModel, object>> expression, bool isCollection = false)
        {
            _relation = relation;
            _expression = expression;
            _isCollection = isCollection;
        }
    }
}