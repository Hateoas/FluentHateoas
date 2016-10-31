using System;
using System.Collections.Generic;
using System.Linq;
using FluentHateoas.Registration;

namespace FluentHateoas.Handling
{
    public class ConfigurationProvider : IConfigurationProvider
    {
        private readonly System.Web.Http.HttpConfiguration _httpConfiguration;
        private readonly ILinkFactory _linkFactory;

        private readonly Dictionary<Type, Func<ConfigurationProvider, object, IEnumerable<IHateoasLink>>> _cachedGetLinksForMethod;

        public ConfigurationProvider(System.Web.Http.HttpConfiguration httpConfiguration, ILinkFactory linkFactory)
        {
            _httpConfiguration = httpConfiguration;
            _linkFactory = linkFactory;

            _cachedGetLinksForMethod = new Dictionary<Type, Func<ConfigurationProvider, object, IEnumerable<IHateoasLink>>> ();
        }

        public IEnumerable<IHateoasLink> GetLinksFor<TModel>(TModel data)
        {
            var registrations = _httpConfiguration
                .GetRegistrationsFor<TModel>()
                .Where(p => !p.IsCollection);
            return _linkFactory.CreateLinks(registrations, data);
        }

        public IEnumerable<IHateoasLink> GetLinksFor<TModel>(IEnumerable<TModel> data)
        {
            var registrations = _httpConfiguration
                .GetRegistrationsFor<TModel>()
                .Where(p => p.IsCollection);

            return _linkFactory.CreateLinks(registrations, data);
        }

        public IEnumerable<IHateoasLink> GetLinksFor(Type contentType, object content)
        {
            Func<ConfigurationProvider, object, IEnumerable<IHateoasLink>> getLinksForFunction;
            if (!_cachedGetLinksForMethod.TryGetValue(contentType, out getLinksForFunction))
                _cachedGetLinksForMethod[contentType] = getLinksForFunction = this.GetLinksForFunc(contentType, content);

            return getLinksForFunction(this, content);
        }
    }
}