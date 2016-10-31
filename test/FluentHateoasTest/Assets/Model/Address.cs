using System;
using System.Diagnostics.CodeAnalysis;

namespace FluentHateoasTest.Assets.Model
{
    [ExcludeFromCodeCoverage]
    public class Address
    {
        public Guid Id { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class CreateAddressRequest
    {
        public string Street { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

    }

    [ExcludeFromCodeCoverage]
    public class UpdateAddressRequest
    {
        public Guid Id { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}