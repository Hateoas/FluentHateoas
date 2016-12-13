using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using FluentHateoas.Interfaces;

namespace FluentHateoasTest.Assets.Model
{
    [ExcludeFromCodeCoverage]
    public class TestRegistration<TModel> : IHateoasRegistration<TModel>
    {
        public TestRegistration(string relation, bool isCollection, bool isMember)
        {
            Model = typeof(TModel);
            Relation = relation;
            IsCollection = isCollection;
            IsMember = isMember;
        }

        public Type Model { get; }
        public string Relation { get; }
        public bool IsCollection { get; }
        public bool IsMember { get; }

        IHateoasExpression<TModel> IHateoasRegistration<TModel>.Expression { get; set; }
        public Expression<Func<TModel, object>>[] ArgumentDefinitions { get; }
        public IHateoasExpression Expression { get; set; }
    }
}