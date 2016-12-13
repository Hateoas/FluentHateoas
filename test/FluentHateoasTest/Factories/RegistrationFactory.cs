using System.Diagnostics.CodeAnalysis;
using FluentHateoasTest.Assets.Model;

namespace FluentHateoasTest.Factories
{
    [ExcludeFromCodeCoverage]
    public static class RegistrationFactory
    {
        public static TestRegistration<TModel> Create<TModel>(string relation, bool isCollection, bool isRelationship)
        {
            return new TestRegistration<TModel>(relation, isCollection, isRelationship);
        }
    }
}