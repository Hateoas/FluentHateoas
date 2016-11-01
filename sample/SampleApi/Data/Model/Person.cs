using System;
using System.Diagnostics.CodeAnalysis;

namespace SampleApi.Data.Model
{
    [ExcludeFromCodeCoverage]
    public class Person
    {
        public Guid Id { get; set; }
        public Guid HouseId { get; set; }
        public string Slug { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTimeOffset Birthday { get; set; }
    }

}