using System;
using FluentHateoasTest.Assets.Model;

namespace FluentHateoasTest.Factories
{
    public static class RegistrationFactory
    {
        public static TestRegistration<TModel> Create<TModel>(string relation, bool isCollection)
        {
            return new TestRegistration<TModel>(relation, isCollection);
        }
    }
}