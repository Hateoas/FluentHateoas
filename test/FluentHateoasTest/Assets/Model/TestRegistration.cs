using System;
using System.Linq.Expressions;
using FluentHateoas.Interfaces;

namespace FluentHateoasTest.Assets.Model
{
    public class TestRegistration<TModel> : IHateoasRegistration<TModel>
    {
        public TestRegistration(string relation, bool isCollection)
        {
            Model = typeof(TModel);
            Relation = relation;
            IsCollection = isCollection;
        }

        public Type Model { get; }
        public string Relation { get; }
        public bool IsCollection { get; }

        IHateoasExpression<TModel> IHateoasRegistration<TModel>.Expression { get; set; }
        public Expression<Func<TModel, object>> ArgumentDefinition { get; }
        public IHateoasExpression Expression { get; set; }
    }
}