using System.Collections.Generic;
using FluentHateoas.Builder.Model;

namespace FluentHateoas.Handling
{
    public interface IHateoasCommand
    {
        string Name { get; set; }
        string Type { get; set; }
        ICollection<Property> Properties { get; set; }
    }
}