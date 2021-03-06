﻿using System;
using System.Diagnostics.CodeAnalysis;

namespace FluentHateoasTest.Assets.Model
{
    [ExcludeFromCodeCoverage]
    public class Person
    {
        public Guid Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTimeOffset Birthday { get; set; }
        public Guid HouseId { get; set; }
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