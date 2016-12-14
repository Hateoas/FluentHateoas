using System.Collections.Generic;
using System.Collections.Specialized;

namespace FluentHateoas.Handling
{
    public class JsonApiResponse
    {
        internal JsonApiResponse() { }

        public object Data { get; set; }
        public List<JsonApiRelation> Includes { get; set; }
    }

    public class JsonApiEntity : JsonApiRelation
    {
        public Dictionary<string, JsonApiRelation> Relationships { get; set; }
        public Dictionary<string, string> Links { get; set; }
    }

    public class JsonApiRelation : JsonApiSimpleRelation
    {
        public Dictionary<string, object> Attributes { get; set; }
    }
    public class JsonApiSimpleRelation
    {
        public string Type { get; set; }
        public string Id { get; set; }
    }
}