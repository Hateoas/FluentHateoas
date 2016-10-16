using System;
using System.Linq.Expressions;
using FluentHateoas.Contracts;

namespace FluentHateoas.Registration
{
    using FluentHateoas.Interfaces;

    public class HateoasRegistration<TModel> : IHateoasRegistration<TModel>
    {
        public Type Model { get; }
        public string Relation { get; }
        public Expression<Func<TModel, object>>[] ArgumentDefinitions { get; }
        public bool IsCollection { get; }

        IHateoasExpression IHateoasRegistration.Expression
        {
            get { return Expression; }
            set { Expression = (IHateoasExpression<TModel>)value; }
        }

        public IHateoasExpression<TModel> Expression { get; set; }

        private readonly IHateoasContainer _container;

        public HateoasRegistration(IHateoasContainer container)
            : this("self", null, container, false)
        {
        }

        public HateoasRegistration(string relation, IHateoasContainer container)
            : this(relation, null, container, false)
        {
        }

        public HateoasRegistration(string relation, Expression<Func<TModel, object>>[] argumentDefinitions, IHateoasContainer container, bool isCollection = false)
        {
            Model = typeof(TModel);
            Relation = relation;
            ArgumentDefinitions = argumentDefinitions;
            IsCollection = isCollection;

            _container = container;
        }

        public void Update()
        {
            _container.Update(this);
        }
    }
}