using System.Collections.Generic;
namespace FluentHateoas.Handling
{
    public class HateoasResponse
    {
        internal HateoasResponse() { }

        public object Data { get; set; }
        public IEnumerable<object> Links { get; set; }
        public IEnumerable<IHateoasCommand> Commands { get; set; }
    }
}