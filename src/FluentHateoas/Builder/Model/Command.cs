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

    public class IntegerProperty : Property
    {
        public int? Min { get; set; }
        public int? Max { get; set; }
        public int? Default { get; set; }
    }
}