using System;
using System.Collections.Generic;
using System.Linq;
using FluentHateoas.Contracts;
using FluentHateoas.Interfaces;

namespace FluentHateoasTest.Assets
{
    public class TestContainer : IHateoasContainer
    {
        private List<IHateoasRegistration> _registrations;
        public IHateoasConfiguration Configuration { get; }

        public TestContainer(IHateoasConfiguration configuration)
        {
            Configuration = configuration;

            _registrations = new List<IHateoasRegistration>();
        }

        public void Add(IHateoasRegistration registration)
        {
            _registrations.Add(registration);
        }

        public void Update()
        {
            throw new System.NotImplementedException();
        }

        public void Update(IHateoasRegistration registration)
        {
            var oldRegistration = _registrations.SingleOrDefault(p => p.Model == registration.Model 
                && p.IsCollection == registration.IsCollection 
                && p.Relation == registration.Relation);

            if (oldRegistration != null)
                _registrations.Remove(oldRegistration);

            _registrations.Add(registration);
        }

        public IHateoasRegistration<TModel> GetRegistration<TModel>(string relation)
        {
            var modelType = typeof (TModel);
            return _registrations.SingleOrDefault(p => p.Model == modelType && p.Relation == relation) as IHateoasRegistration<TModel>;
        }
    }
}