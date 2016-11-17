using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using FluentHateoas.Builder.Model;

namespace FluentHateoas.Handling
{
    // TODO Refactor: Separate LinkBuilder and POCO functionality?
    public class LinkBuilder : ILinkBuilder
    {
        public LinkBuilder(object data)
        {
            Data = data;
            Arguments = new Dictionary<string, Argument>();
        }

        public IHateoasLink Build()
        {
            if (Action == null)
                throw new InvalidOperationException("LinkBuilder.Action property cannot be null");

            if (Method == null)
                throw new InvalidOperationException("LinkBuilder.Method property cannot be null");

            var result = new HateoasLink
            {
                Relation = Relation
            };

            if (IsTemplate)
                result.Template = Arguments.Any(p => p.Value.IsTemplateArgument)
                    ? this.GetPath()
                    : this.GetPathAsTemplate();
            else
                result.LinkPath = this.GetPath();

            result.Method = Method.ToString();

            if (Command != null)
            {
                result.Command = Command;
            }

            return result;
        }

        public object Data { get; }
        public string Relation { get; set; }
        public IDictionary<string, Argument> Arguments { get; set; }
        public System.Type Controller { get; set; }
        public bool Success { get; set; }
        public HttpMethod Method { get; set; }
        public bool IsTemplate { get; set; }
        public bool IsFixed { get; set; }
        public string FixedRoute { get; set; }
        public IHateoasCommand Command { get; set; }
        public MethodInfo Action { get; set; }
    }
}