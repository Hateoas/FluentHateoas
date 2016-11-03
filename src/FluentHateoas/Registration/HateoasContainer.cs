using FluentHateoas.Contracts;
using FluentHateoas.Handling;
using FluentHateoas.Interfaces;

namespace FluentHateoas.Registration
{

    public class HateoasContainer : IHateoasContainer
    {
        internal HateoasContainer(IHttpConfiguration httpConfiguration, HateoasConfiguration configuration)
        {
            Configuration = configuration;
            //Registrations = new List<IHateoasRegistration>();

            _httpConfiguration = httpConfiguration;

            Update();
        }

        public IHateoasConfiguration Configuration { get; }
        //public IList<IHateoasRegistration> Registrations { get; }

        private readonly IHttpConfiguration _httpConfiguration;

        public void Add(IHateoasRegistration registration)
        {
            //Registrations.Add(registration);
            _httpConfiguration.AddRegistration(registration);
        }

        public void Update()
        {
            _httpConfiguration.UpdateConfiguration(Configuration);
            //foreach (var registration in Registrations)
            //{
            //    _httpConfiguration.UpdateRegistration(registration);
            //}
        }

        public void Update(IHateoasRegistration registration)
        {
            // TODO Move registrations to container (again) and always just update container
            //_httpConfiguration.UpdateConfiguration(this);
            _httpConfiguration.UpdateRegistration(registration);
        }
    }
}