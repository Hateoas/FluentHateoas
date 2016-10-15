using FluentAssertions;
using FluentHateoas.Builder.Handlers;
using FluentHateoas.Registration;
using FluentHateoasTest.Assets.Controllers;
using FluentHateoasTest.Assets.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentHateoasTest.Handling
{
    [TestClass]
    public class TemplateHandlerTest : BaseHandlerTest<TemplateHandler>
    {
        [TestMethod]
        public void TemplateHandlerShouldAlwaysProcess()
        {
            Container
                .Register<Person>("create", p => p.Id)
                .Get<PersonController>();

            var registration = Container.GetRegistration<Person>("create");
            Handler.CanProcess(registration, LinkBuilder).Should().BeTrue();
        }

        [TestMethod]
        public void HandlerShouldSetTemplateFlag()
        {
            Container
                .Register<Person>("create", p => p.Id)
                .Get<PersonController>()
                .AsTemplate();

            var registration = Container.GetRegistration<Person>("create");

            Handler.Process(registration, LinkBuilder, Person);
            LinkBuilder.IsTemplate.Should().BeTrue();
        }
    }
}