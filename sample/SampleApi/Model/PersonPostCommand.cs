using FluentHateoas.Attributes;

namespace SampleApi.Model
{
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class PersonPostCommand
    {
        public string Firstname { get; set; }

        [MinValue(0), MaxValue(10)]
        public int Cars { get; set; }
    }
}