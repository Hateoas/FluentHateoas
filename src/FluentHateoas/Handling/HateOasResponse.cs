using System.Collections.Generic;
namespace FluentHateoas.Handling
{
    internal class HateOasResponse
    {
        public object Data { get; set; }
        public IEnumerable<object> Links { get; set; }
        public IEnumerable<IHateoasCommand> Commands { get; set; }
    }
}