using System.Web.Http;
using AutoMapper;
using SampleApi.Model;

namespace SampleApi
{
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Mapper.Initialize(mapper => mapper.CreateMap<Data.Model.Person, Person>());

            // Set dependency resolver
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
