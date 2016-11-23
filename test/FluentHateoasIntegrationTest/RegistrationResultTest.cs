using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using FluentHateoas.Helpers;
using FluentHateoas.Registration;
using FluentHateoasTest.Assets.Controllers;
using FluentHateoasTest.Assets.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluetHateoasIntegrationTest
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class RegistrationResultTest : RegistrationResultTestBase
    {
        [TestInitialize]
        public void TestInitialize()
        {
        }

        [TestMethod]
        public async Task GetSinglePersonRegistrationShouldReturnSingleItemLink()
        {
            // arrange
            RegistrationClass.Registering = container =>
                container
                    .Register<Person>("self", p => p.Id)
                    .Get<PersonController>();

            var person = new Person {Id = Guid.NewGuid()};
            Response.AddContent(person);

            // act & assert
            var result = await RegisterGetAndAssertResponse(true, person, 1, 0);

            // assert
            result.HateoasResponse
                .Links.First()
                .Should().Match((LinkResponse lr) => $"/api/person/{person.Id}".Equals(lr.Href));

            /*
            {
                "Data": {
                    "Id": "3cc857e2-acd2-4780-a73b-5996ffb0ea82",
                    ...
                },
                "Links": [
                    {
                        "Href": "/api/person/3cc857e2-acd2-4780-a73b-5996ffb0ea82",
                        "Template": null,
                        "Rel": "self",
                        "Method": "GET",
                        "Command": null
                    }
                 ],
                 "Commands": []
             }
             */
        }
    }
}