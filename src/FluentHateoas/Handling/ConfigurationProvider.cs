using HttpConfigurationExtensions = FluentHateoas.Registration.HttpConfigurationExtensions;

namespace FluentHateoas.Handling
{
    public class ConfigurationProvider : IConfigurationProvider
    {
        private readonly System.Web.Http.HttpConfiguration _httpConfiguration;
        private readonly ILinkFactory _linkFactory;

        public ConfigurationProvider(System.Web.Http.HttpConfiguration httpConfiguration, ILinkFactory linkFactory)
        {
            _httpConfiguration = httpConfiguration;
            _linkFactory = linkFactory;
        }

        public System.Collections.Generic.IEnumerable<IHateoasLink> GetLinksFor<TModel>(object data)
        {
            var registrations = HttpConfigurationExtensions.GetRegistrationsFor<TModel>(_httpConfiguration);
            return _linkFactory.CreateLinks(registrations, data);
        }

        public System.Collections.Generic.IEnumerable<IHateoasLink> GetLinksFor(System.Type modelType, object data)
        {
            var registrations = HttpConfigurationExtensions.GetRegistrationsFor(_httpConfiguration, modelType);
            throw new System.NotImplementedException();
        }
    }
}