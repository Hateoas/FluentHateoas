using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentHateoas.Contracts;
using FluentHateoas.Interfaces;

namespace FluentHateoasTest.EndToEnd
{
    [ExcludeFromCodeCoverage]
    public class TestContainer : IHateoasContainer
    {
        private readonly List<IHateoasRegistration> _registrations;

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
            var modelType = typeof(TModel);
            return _registrations.SingleOrDefault(p => p.Model == modelType && p.Relation == relation) as IHateoasRegistration<TModel>;
        }

        public List<IHateoasRegistration<TModel>> GetRegistrationsFor<TModel>()
        {
            var modelType = typeof(TModel);
            return _registrations.Where(p => p.Model == modelType).Select(p => p as IHateoasRegistration<TModel>).ToList();
        }
    }
}