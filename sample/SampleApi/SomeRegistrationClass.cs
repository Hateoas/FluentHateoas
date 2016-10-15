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

            // =====================================================================================================================
            //  get all persons link
            // =====================================================================================================================
            container
                .Register<Person>("self")
                .Get<PersonController>();

            //  {
            //      "rel": "self"
            //      "href": "/api/person/A2209F1A-4607-4B40-AC59-EC7DAFC6D1DF"
            //  }


            // =======================================================================================================================
            //  get all persons link with custom function registration
            // =======================================================================================================================
            container
                .Register<Person>("self")
                .Get<PersonController>(p => p.GetAll);

            //  {
            //      "rel": "self"
            //      "href": "/api/person/"
            //  }


            // =======================================================================================================================
            //  get all persons link with custom function implementation registration (TODO: invent provider to make this one useful)
            // =======================================================================================================================
            container
                .Register<Person>("self")
                .Get<PersonController>(p => p.GetAllWithParams(string.Empty));

            //  {
            //      "rel": "self"
            //      "href": "/api/person/getallwithparams/"
            //  }


            // =======================================================================================================================
            //  get single person link
            // =======================================================================================================================
            container
                .Register<Person>("self", p => p.Id)
                .Get<PersonController>();

            //  {
            //      "rel": "self"
            //      "href": "/api/person/42DB0C7B-1F89-41E5-B485-880176EDBE83"
            //  }


            // =======================================================================================================================
            //  get single person link with custom function registration
            // =======================================================================================================================
            container
                .Register<Person>("self", p => p.Id)
                .Get<PersonController>(p => p.GetPerson);

            //  {
            //      "rel": "self"
            //      "href": "/api/person/42DB0C7B-1F89-41E5-B485-880176EDBE83"
            //  }


            // get single person link as template (e.g. '/persons/:id')
            container
                .Register<Person>("self", p => p.Id)
                .Get<PersonController>()
                .AsTemplate();

            //  {
            //      "rel": "self"
            //      "href": "/api/person/:id"
            //  }


            // =======================================================================================================================
            //  Specify the GET-message expects a collection
            // =======================================================================================================================
            container
                .Register<Person>("item")
                .Get<PersonController>()
                .AsCollection()
                .AsTemplate(p => p.Id, p => p.Slug);

            //  {
            //      "rel": "item"
            //      "href": "/api/person/:id/:slug"
            //  }


            // =======================================================================================================================
            //  Conditional dynamic parameter
            // =======================================================================================================================
            container
                .Register<Person>("next")
                .Get<PersonController>()
                .When<IPersonProvider>((provider, person) => provider.HasNextId(person))
                .With<IPersonProvider>((provider, person) => provider.GetNextId(person));

            //  {
            //      "rel": "next"
            //      "href": "/api/person/C1B837B0-5FDC-495F-9847-3ABF68E0B96E"
            //  }


            ////
            //// POST REGISTRATIONS
            ////

            // =======================================================================================================================
            //  Default post rendering
            // =======================================================================================================================
            container
                .Register<Person>("create")
                .Post<PersonController>();

            //  {
            //      "rel": "create"
            //      "href": "/api/person/EADCB057-A41D-448B-B10D-94F99162AD4E"
            //  }


            // =======================================================================================================================
            //  Example posted void
            // =======================================================================================================================
            container
                .Register<Person>("self", p => p.Id)
                .Delete<PersonController>(p => p.Delete);

            //  {
            //      "rel": "create"
            //      "method": "POST"
            //      "href": "/api/person/EADCB057-A41D-448B-B10D-94F99162AD4E"
            //  }


            // =======================================================================================================================
            //  Posting a command
            // =======================================================================================================================
            container
                .Register<Person>("create")
                .Post<PersonController>()
                .WithCommand<PersonPostCommand>();

            //  {
            //      "rel": "create"
            //      "method": "POST"
            //      "command": "createPerson"
            //      "href": "/api/person/EADCB057-A41D-448B-B10D-94F99162AD4E"
            //  }
            //  {
            //      "command": "createPerson"
            //      "template": {
            //          "type": "PersonPostCommand",
            //          "properties": [
            //           {
            //              "name": "firstname"
            //              "type": "string"
            //           },
            //           {
            //              "name": "lastname"
            //              "type": "string"
            //           },
            //           {
            //              "name": "gender"
            //              "type": "select"
            //              "selected": "male"
            //              "options": [ "male", "female" ]
            //           },
            //           {
            //              "name": "location"
            //              "type": "select"
            //              "readonly": "true"
            //           },
            //           {
            //              "name": "remember"
            //              "type": "boolean"
            //              "default": "true"
            //           },
            //           {
            //              "name": "cars"
            //              "type": "integer",
            //              "min": 0,
            //              "max": 10,
            //              "default": 1
            //           },
            //           {
            //              "name": "birthday"
            //              "type": "date",
            //              "min": "1900-01-01",
            //              "max": "2016-10-15", //<== now
            //              "format": "yyyy-MM-dd"
            //           }                    
            //          ]
            //      }
            //  }


            // =======================================================================================================================
            //  Post a dynamic template
            // =======================================================================================================================
            container
                .Register<Person>("add-address", p => p.Id)
                .Post<AddressController>()
                .WithCommand<ITemplateFactory>(p => p.Create());

            ////
            //// PUT REGISTRATIONS
            ////


            // =======================================================================================================================
            // =======================================================================================================================
            container
                .Register<Person>("self", p => p.Id)
                .Put<PersonController>();


            // =======================================================================================================================
            // =======================================================================================================================
            container
                .Register<Person>("self", p => p.Id)
                .Put<PersonController>()
                .WithCommand<PersonPostCommand>();

            ////
            //// DELETE REGISTRATIONS
            ////

            
            // =======================================================================================================================
            // =======================================================================================================================
            container
                .Register<Person>("self", p => p.Id)
                .Delete<PersonController>();

            // store container

        }
    }
}