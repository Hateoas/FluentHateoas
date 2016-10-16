using System.Linq.Expressions;
using System.Reflection;

namespace FluentHateoas.Handling
{
    public class LinkBuilder
    {
        public LinkBuilder(object data)
        {
            Data = data;
        }

        public object Data { get; private set; }
        public string Relation { get; set; }
        public object Argument { get; set; }
        public System.Type Controller { get; set; }
        public bool Success { get; set; }
        public System.Net.Http.HttpMethod Method { get; set; }
        public bool IsTemplate { get; set; }
        public bool IsFixed { get; set; }
        public string FixedRoute { get; set; }
        public IHateoasCommand Command { get; set; }
        public MethodInfo Action { get; set; }
    }
}