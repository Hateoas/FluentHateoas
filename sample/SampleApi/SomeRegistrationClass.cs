using System.Collections.Generic;
using System.Linq;
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

            container
                .Register<Person>("self", p => p.Id)
                .Get<PersonController>();

            container
                .Register<Person>("self", p => p.Id)
                .Get<PersonController>()
                .AsTemplate();

            container
                .Register<IEnumerable<Person>>("item")
                .Get<PersonController>();
                //.AsTemplate(p => p.Id, p => p.Slug);

            //container
            //    .Register<Person>("self", p => p.Id)
            //    .Get<PersonController>(p => p.GetPerson);

            //container
            //    .Register<Person>("self", p => p.Id)
            //    .Post<PersonController>();

            //container
            //    .Register<Person>("next")
            //    .Get<PersonController>()
            //    .When<IPersonProvider>((provider, person) => provider.HasNextId(person.id))
            //    .With<IPersonProvider>((provider, person) => provider.GetNextId(person.id));

            //container
            //    .Register<Person>("self", p => p.Id)
            //    .Post<PersonController>(p => p.AddPerson);

            //container
            //    .Register<Person>("self", p => p.Id)
            //    .Post<PersonController>()
            //    .Command<PersonPostCommand>();

            //container
            //    .Register<Person>("add-address", p => p.Id)
            //    .Post<AddressController>()
            //    .Command<ITemplateFactory>(p => p.Create());
        }
    }
}