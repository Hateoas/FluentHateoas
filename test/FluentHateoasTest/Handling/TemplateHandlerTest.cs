using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using FluentHateoas.Builder.Handlers;
using FluentHateoas.Registration;
using FluentHateoasTest.Assets.Controllers;
using FluentHateoasTest.Assets.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentHateoasTest.Handling
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class TemplateHandlerTest : BaseHandlerTest<TemplateHandler>
    {
        [TestInitialize]
        public void Initialize()
        {
            Handler = new TemplateHandler();
        }

        [TestMethod]
        public void TemplateHandlerShouldAlwaysProcess()
        {
            // arrange
            var registration = GetRegistration<Person, PersonController>(p => p.Id);

            // act & assert
            Handler.CanProcess(registration, LinkBuilder).Should().BeTrue();
        }

        [TestMethod]
        public void HandlerShouldSetTemplateFlag()
        {
            // arrange
            var registration = GetRegistration<Person, PersonController>(p => p.Id, template: true);

            // act
            Handler.Process(registration, LinkBuilder, Person);

            // assert
            LinkBuilder.IsTemplate.Should().BeTrue();
        }
    }
}