using System;

namespace SampleApi.Model
{
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class Person
    {
        public Guid Id { get; set; }
        public string Slug { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
    }
}