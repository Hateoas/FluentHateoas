using System;
using System.Collections.Generic;
using System.Linq;
using FluentHateoas.Helpers;
using FluentHateoas.Registration;

namespace FluentHateoas.Handling
{
    public class ConfigurationProvider : IConfigurationProvider
    {
        private readonly IHttpConfiguration _httpConfiguration;
        private readonly ILinkFactory _linkFactory;

        private readonly ICache<Type, Func<ConfigurationProvider, object, IEnumerable<IHateoasLink>>> _getLinksForMethodCache;
        private readonly IConfigurationProviderGetLinksForFuncProvider _linksForFuncProvider;

        public ConfigurationProvider(IHttpConfiguration httpConfiguration, ILinkFactory linkFactory, IConfigurationProviderGetLinksForFuncProvider linksForFuncProvider, ICache<Type, Func<ConfigurationProvider, object, IEnumerable<IHateoasLink>>> getLinksForMethodCache)
        {
            _httpConfiguration = httpConfiguration;
            _linkFactory = linkFactory;
            _linksForFuncProvider = linksForFuncProvider;
            _getLinksForMethodCache = getLinksForMethodCache;
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

        public NullValueHandling GetNullValueHandling()
        {
            return _httpConfiguration.GetConfiguration().NullValueHandling;
        }

        public IEnumerable<IHateoasLink> GetLinksFor(Type contentType, object content)
        {
            // TODO Dynamically creating/caching those link functions is heavy and should move to the registration phase.
            var getLinksForFunction = _getLinksForMethodCache.Get(contentType);
            if (getLinksForFunction == null)
                _getLinksForMethodCache.Add(
                    contentType,
                    getLinksForFunction = _linksForFuncProvider.GetLinksForFunc(this, contentType, content)
                );
            return getLinksForFunction(this, content.Materialize());
        }
    }
}