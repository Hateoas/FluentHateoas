using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FluentHateoas.Interfaces;
using FluentHateoas.Registration;

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

        public IEnumerable<IHateoasLink> GetLinksFor<TModel>(TModel data)
        {
            var isCollection = typeof(TModel).GetInterfaces().Contains(typeof(IEnumerable));
            var registrations = _httpConfiguration.GetRegistrationsFor<TModel>().Where(p => p.IsCollection == isCollection);

            if (!isCollection)
                return _linkFactory.CreateLinks(registrations.Cast<IHateoasRegistration<TModel>>().ToList(), data);

            var yesThisIsVeryHacky = typeof(IHateoasRegistration<>).MakeGenericType(typeof(TModel).GenericTypeArguments[0]);
            var soInCaseOfBetterIdeas = typeof(Enumerable).GetMethod("Cast").MakeGenericMethod(yesThisIsVeryHacky).Invoke(null, new object[] { registrations });
            var pleaseRefactorThis = (typeof(Enumerable).GetMethod("ToList")).MakeGenericMethod(yesThisIsVeryHacky).Invoke(null, new[] { soInCaseOfBetterIdeas });
            return (IEnumerable<IHateoasLink>) _linkFactory.GetType().GetMethod(nameof(_linkFactory.CreateLinks)).MakeGenericMethod(typeof(TModel).GenericTypeArguments[0]).Invoke(_linkFactory, new [] { pleaseRefactorThis, data });
        }
    }
}