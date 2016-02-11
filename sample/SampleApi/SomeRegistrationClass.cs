using FluentHateoas.Contracts;
using FluentHateoas.Registration;
using SampleApi.ApiController;
using SampleApi.Model;

namespace SampleApi
{
    public class SomeRegistrationClass : IHateoasRegistrationProfile
    {
        public void Register(IHateoasContainer container)
        {
            container.Configure(new {
                HrefStyle = HrefStyle.Relative,
                LinkStyle = LinkStyle.Array,
                TemplateStyle = TemplateStyle.Rendered
            });

            container
                .Register<Person>("self")
                .Get<PersonController>();
        }
    }
}