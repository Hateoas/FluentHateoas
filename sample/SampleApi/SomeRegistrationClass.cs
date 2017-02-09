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
                    TemplateStyle = TemplateStyle.Rendered,
                    ResponseStyle = ResponseStyle.JsonApi,
                    NullValueHandling = NullValueHandling.Ignore
                });

            ////
            //// GET REGISTRATIONS
            ////

            // =====================================================================================================================
            //  get all persons link
            // =====================================================================================================================
            container
                .Register<Person>("default-list")
                .Get<PersonController>();

            //  {
            //      "rel": "self"
            //      "href": "/api/person/
            //  }


            // =======================================================================================================================
            //  get all persons link with custom function registration => IS HIDDEN WHEN TESTING UNAUTHORIZED
            // =======================================================================================================================
            container
                .Register<Person>("get-all")
                .Get<PersonController>(p => p.GetAll);

            //  {
            //      "rel": "self"
            //      "href": "/api/person/"
            //  }

            // =======================================================================================================================
            //  get all persons link with custom function registration
            // =======================================================================================================================
            container
                .Register<Person>("multiple-id", p => p.Id, p => p.HouseId)
                .Get<AddressController>(p => p.Get);

            // Should ideally be:
            //container
            //    .Register<Person>("multiple-id", p => p.Id, p => p.HouseId)
            //    .Get<AddressController>(p => p.Get);

            //  {
            //      "rel": "self"
            //      "href": "/api/address/92B27E6E-0F34-4732-B50C-A33114EF9053/E1F831BB-7677-4817-AB1F-F400D2CB9F99"
            //  }


            // =======================================================================================================================
            //  get all persons link with custom function implementation registration (TODO: invent provider to make this one useful)
            // =======================================================================================================================
            //container
            //    .Register<Person>("self-3")
            //    .Get<PersonController>(p => p.GetAllWithParams(string.Empty));

            //  {
            //      "rel": "self"
            //      "href": "/api/person/getallwithparams/"
            //  }


            // =======================================================================================================================
            //  get single person link
            // =======================================================================================================================
            container
                .RegisterCollection<Person>("get-by-id", p => p.Id)
                .Get<PersonController>()
                .AsTemplate(p => p.Id);

            //  {
            //      "rel": "self"
            //      "href": "/api/person/96d74b0d-4456-4643-a5fd-d0a31af0c284",
            //  }


            // =======================================================================================================================
            //  get single person link with custom function registration
            // =======================================================================================================================
            container
                .Register<Person>("get-by-id-with-action", p => p.Id)
                .Get<PersonController>(p => p.GetById);

            //  {
            //      "rel": "self"
            //      "href": "/api/person/96d74b0d-4456-4643-a5fd-d0a31af0c284",
            //  }


            // get single person link as template (e.g. '/persons/:id')
            container
                .Register<Person>("get-by-id-template", p => p.Id)
                .Get<PersonController>()
                .AsTemplate();

            //  {
            //      "rel": "self"
            //      "href": "/api/person/:id"
            //  }


            // =======================================================================================================================
            //  Specify the GET-message expects a collection ==> todo: template is failing
            // =======================================================================================================================
            container
                .RegisterCollection<Person>("item")
                .Get<PersonController>()
                .AsTemplate(p => p.Id, p => p.Slug);

            //  {
            //      "rel": "item"
            //      "href": "/api/person/:id/:slug"
            //  }


            // =======================================================================================================================
            //  Register relationship in collection
            // =======================================================================================================================
            container
                .RegisterCollection<Person>(p => p.Dad, p => p.DadId)
                .Get<PersonController>()
                .AsTemplate(p => p.DadId);

            container
                .RegisterCollection<Person>(p => p.Mom)
                .Get<PersonController>()
                .AsTemplate(p => p.MomId);


            //  {
            //      "rel": "item"
            //      "href": "/api/person/:id/:slug"
            //  }


            // =======================================================================================================================
            //  Conditional dynamic parameter
            // =======================================================================================================================
            container
                .Register<Person>("next-5B8DC86A-72A2-40E8-BDA7-EF35FBD26399")
                .Get<PersonController>(p => p.GetById)
                .When<IPersonProvider>((provider, person) => provider.HasNextId(person))
                .IdFrom<IPersonProvider>((provider, person) => provider.GetNextId(person));

            container
                .Register<Person>("previous-A1557C62-2BA5-402D-A879-EB17E811EDD0")
                .Get<PersonController>(p => p.GetById)
                .IdFrom<IPersonProvider>((provider, person) => provider.GetPreviousId(person));

            //  {
            //      "rel": "next"
            //      "href": "/api/person/C1B837B0-5FDC-495F-9847-3ABF68E0B96E"
            //  }

            // =======================================================================================================================
            //  Get where Controller action "Get" share list and get-by-id
            // =======================================================================================================================
            container
                .Register<Person>("share-get")
                .Get<AddressController>(p => p.Get);

            //  {
            //      "rel": "share-get"
            //      "href": "/api/person"
            //  }

            // todo: template is failing

            container
                .Register<Person>("share-get-with-id", p => p.Id, p => p.HouseId)
                .Get<AddressController>(p => p.Get);

            //  {
            //      "rel": "share-get-with-id"
            //      "href": "/api/person/C1B837B0-5FDC-495F-9847-3ABF68E0B96E"
            //  }

            // =======================================================================================================================
            //  Get where Controller action "Get" share list and get-by-id
            // =======================================================================================================================
            container
                .Register<Person>(p => p.Dad, p => p.DadId)
                .Get<AddressController>(p => p.Get);

            container
                .Register<Person>(p => p.RelatedPersons)
                .Get<AddressController>(p => p.Get);

            container
                .Register<Person>(p => p.Mom)
                .Get<AddressController>(p => p.Get);

            //  {
            //      "rel": "dad"
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
                .Register<Person>("delete-with-void", p => p.Id)
                .Delete<PersonController>(p => p.Delete);

            //  {
            //      "rel": "create"
            //      "method": "POST"
            //      "href": "/api/person/96d74b0d-4456-4643-a5fd-d0a31af0c284"
            //  }


            // =======================================================================================================================
            //  Posting a command
            // =======================================================================================================================
            container
                .Register<Person>("create-with-command")
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
            //  Post a dynamic template todo: not working
            // =======================================================================================================================
            //container
            //    .Register<Person>("add-address-with-dynamic-command", p => p.Id)
            //    .Post<AddressController>()
            //    .WithCommand<ITemplateFactory>(p => p.Create());

            ////
            //// PUT REGISTRATIONS
            ////


            // =======================================================================================================================
            //  Default PUT-action todo: get the getbyid action
            // =======================================================================================================================
            container
                .Register<Person>("put-action")
                .Put<PersonController>();


            // =======================================================================================================================
            // =======================================================================================================================
            //container
            //    .Register<Person>("self-10", p => p.Id)
            //    .Put<PersonController>()
            //    .WithCommand<PersonPostCommand>();

            ////
            //// DELETE REGISTRATIONS
            ////


            // =======================================================================================================================
            // =======================================================================================================================
            //container
            //    .Register<Person>("self-11", p => p.Id)
            //    .Delete<PersonController>();

            // store container

            container
                .Register<Person>("cars", p => p.Id)
                .Get<CarController>(p => p.GetByPersonId);
        }
    }
}