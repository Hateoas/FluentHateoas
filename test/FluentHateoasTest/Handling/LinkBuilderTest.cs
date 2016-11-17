using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using FluentAssertions;
using FluentHateoas.Builder.Model;
using FluentHateoas.Handling;
using FluentHateoasTest.Assets.Controllers;
using FluentHateoasTest.Assets.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentHateoasTest.Handling
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class LinkBuilderTest
    {
        private LinkBuilder _linkBuilder;
        private Person _person;

        [TestInitialize]
        public void TestInitialize()
        {
            _person = new Person
            {
                Id = Guid.NewGuid()
            };
            _linkBuilder = new LinkBuilder(_person)
            {
                Relation = "self"
            };
        }

        [TestMethod]
        public void BuildShouldThrowExceptionWhenNoActionSpecified()
        {
            // arrange
            Action action = () => _linkBuilder.Build();

            // act & assert
            action
                .ShouldThrow<InvalidOperationException>()
                .And.Message.Should().Be("LinkBuilder.Action property cannot be null");
        }

        [TestMethod]
        public void BuildShouldThrowExceptionWhenNoMethodSpecified()
        {
            // arrange
            _linkBuilder.Action = typeof(PersonController).GetMethod(nameof(PersonController.GetParents));

            Action action = () => _linkBuilder.Build();

            // act & assert
            action
                .ShouldThrow<InvalidOperationException>()
                .And.Message.Should().Be("LinkBuilder.Method property cannot be null");
        }

        [TestMethod]
        public void BuildShouldReturnHateoasLink()
        {
            // arrange
            _linkBuilder.Action = typeof(PersonController).GetMethod(nameof(PersonController.GetParents));
            _linkBuilder.Method = HttpMethod.Get;
            _linkBuilder.IsTemplate = false;

            // act & assert
            _linkBuilder
                .Build()
                .Should().NotBeNull()
                .And.BeOfType<HateoasLink>()
                .And.Match((HateoasLink link) => _linkBuilder.Relation.Equals(link.Relation))
                .And.Match((HateoasLink link) => "/api/person/{id}/parents".Equals(link.LinkPath))
                .And.Match((HateoasLink link) => link.Template == null)
                .And.Match((HateoasLink link) => HttpMethod.Get.ToString().Equals(link.Method))
                .And.Match((HateoasLink link) => link.Command == null)
;
        }

        [TestMethod]
        public void BuildShouldReturnHateoasLinkWhenIsTemplateIsTrueAndNoTemplateArguments()
        {
            // arrange
            _linkBuilder.Action = typeof(PersonController).GetMethod(nameof(PersonController.Put));
            _linkBuilder.Method = HttpMethod.Put;
            _linkBuilder.IsTemplate = true;
            _linkBuilder.Command = new Command();

            // act & assert
            _linkBuilder
                .Build()
                .Should().NotBeNull()
                .And.BeOfType<HateoasLink>()
                .And.Match((HateoasLink link) => _linkBuilder.Relation.Equals(link.Relation))
                .And.Match((HateoasLink link) => "/api/person".Equals(link.Template))
                .And.Match((HateoasLink link) => link.LinkPath == null)
                .And.Match((HateoasLink link) => HttpMethod.Put.ToString().Equals(link.Method))
                .And.Match((HateoasLink link) => _linkBuilder.Command.Equals(link.Command));
        }

        [TestMethod]
        public void BuildShouldReturnHateoasLinkWhenIsTemplateIsTrueAndTemplateArguments()
        {
            // arrange
            _linkBuilder.Action = typeof(PersonController).GetMethod(nameof(PersonController.GetParents));
            _linkBuilder.Method = HttpMethod.Get;
            _linkBuilder.IsTemplate = true;
            _linkBuilder.Arguments.Add("id", new Argument { IsTemplateArgument = true, Value = _person.Id });

            // act & assert
            _linkBuilder
                .Build()
                .Should().NotBeNull()
                .And.BeOfType<HateoasLink>();
        }
    }
}