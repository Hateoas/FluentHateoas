using System;
using System.Collections;
using System.Collections.Generic;
using FluentHateoas.Handling;

namespace FluentHateoas.Builder.Model
{
    public class Command : IHateoasCommand
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public ICollection<Property> Properties { get; set; }
    }

    public class Property
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public bool? Required { get; set; }
        public int? Order { get; set; }
    }
}