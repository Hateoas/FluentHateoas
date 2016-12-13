using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using FluentHateoas.Builder.Model;

namespace FluentHateoas.Handling
{
    public interface ILinkBuilder
    {
        IHateoasLink Build();

        MethodInfo Action { get; set; }
        IDictionary<string, Argument> Arguments { get; set; }
        IHateoasCommand Command { get; set; }
        Type Controller { get; set; }
        object Data { get; }
        string FixedRoute { get; set; }
        bool IsFixed { get; set; }
        bool IsTemplate { get; set; }
        bool IsMember { get; set; }
        HttpMethod Method { get; set; }
        string Relation { get; set; }
        bool Success { get; set; }
    }
}