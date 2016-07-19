namespace SampleApi
{
    using System.Diagnostics.CodeAnalysis;

    using FluentHateoas.Contracts;
    using FluentHateoas.Registration;

    using SampleApi.ApiController;
    using SampleApi.Model;
    using SampleApi.Providers;

    [ExcludeFromCodeCoverage]
    public class SomeRegistrationClass : IHateoasRegistrationProfile
    {
        public void Register(IHateoasContainer container)
        {
            container.Configure(
                new
                {
                    HrefStyle = HrefStyle.Relative,
                    LinkStyle = LinkStyle.Array,
                    TemplateStyle = TemplateStyle.Rendered
                });

            ////
            //// GET REGISTRATIONS
            ////

            // get all persons link
            container
                .Register<Person>("self")
                .Get<PersonController>();

            // get all persons link with custom function registration
            container
                .Register<Person>("self")
                .Get<PersonController>(p => p.GetAll);

            // get all persons link with custom function implementation registration (TODO: invent provider to make this one useful)
            container
                .Register<Person>("self")
                .Get<PersonController>(p => p.GetAllWithParams(string.Empty));

            // get single person link
            container
                .Register<Person>("self", p => p.Id)
                .Get<PersonController>();

            // get single person link with custom function registration
            container
                .Register<Person>("self", p => p.Id)
                .Get<PersonController>(p => p.GetPerson);

            // get single person link as template (e.g. '/persons/:id')
            container
                .Register<Person>("self", p => p.Id)
                .Get<PersonController>()
                .AsTemplate();

            container
                .Register<Person>("item")
                .Get<PersonController>()
                .AsCollection()
                .AsTemplate(p => p.Id, p => p.Slug);

            container
                .Register<Person>("next")
                .Get<PersonController>()
                .When<IPersonProvider>((provider, person) => provider.HasNextId(person))
                .With<IPersonProvider>((provider, person) => provider.GetNextId(person));

            ////
            //// POST REGISTRATIONS
            ////

            container
                .Register<Person>("self", p => p.Id)
                .Post<PersonController>();

            ////container
            ////    .Register<Person>("self", p => p.Id)
            ////    .Post<PersonController>(p => p.AddPerson);

            container
                .Register<Person>("self", p => p.Id)
                .Post<PersonController>()
                .WithCommand<PersonPostCommand>();

            container
                .Register<Person>("add-address", p => p.Id)
                .Post<AddressController>()
                .WithCommand<ITemplateFactory>(p => p.Create());

            ////
            //// PUT REGISTRATIONS
            ////

            container
                .Register<Person>("self", p => p.Id)
                .Put<PersonController>();

            container
                .Register<Person>("self", p => p.Id)
                .Put<PersonController>()
                .WithCommand<PersonPostCommand>();

            ////
            //// DELETE REGISTRATIONS
            ////

            container
                .Register<Person>("self", p => p.Id)
                .Delete<PersonController>();
        }
    }
}