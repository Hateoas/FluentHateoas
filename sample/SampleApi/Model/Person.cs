using System;

namespace SampleApi.Model
{
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class Person
    {
        public Guid Id { get; set; }
        public Guid HouseId { get; set; }
        public string Slug { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTimeOffset Birthday { get; set; }

        public Guid MomId { get; set; }
        public Person Mom { get; set; }
        public Guid DadId { get; set; }
        public Person Dad { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class CreatePersonRequest
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTimeOffset Birthday { get; set; }
    }
    [ExcludeFromCodeCoverage]
    public class CreateChildRequest : CreatePersonRequest
    {
        public Guid Id { get; set; }
        public Guid Mom { get; set; }
        public Guid Dad { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class UpdatePersonRequest
    {
        public Guid Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTimeOffset Birthday { get; set; }
    }
}