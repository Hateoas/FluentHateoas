using System;
using FluentHateoas.Interfaces;

namespace FluentHateoasTest.Assets.Model
{
    public class TestRegistration : IHateoasRegistration
    {
        public TestRegistration(Type model, string relation, bool isCollection)
        {
            Model = model;
            Relation = relation;
            IsCollection = isCollection;
        }

        public Type Model { get; }
        public string Relation { get; }
        public bool IsCollection { get; }
        public IHateoasExpression Expression { get; set; }
    }
}