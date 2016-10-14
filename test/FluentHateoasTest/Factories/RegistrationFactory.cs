using System;
using FluentHateoasTest.Assets.Model;

namespace FluentHateoasTest.Factories
{
    public static class RegistrationFactory
    {
        public static TestRegistration Create(Type type, string relation, bool isCollection)
        {
            return new TestRegistration(type, relation, isCollection);
        }
    }
}