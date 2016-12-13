using System.Collections.Generic;
using System.Collections.Specialized;

namespace FluentHateoas.Handling
{
    public class JsonApiResponse
    {
        internal JsonApiResponse() { }

        public JsonApiEntity Data { get; set; }
    }

    public class JsonApiEntity
    {
        public string Type { get; set; }
        public string Id { get; set; }
        public Dictionary<string, JsonApiEntity> Relationships { get; set; }
        public Dictionary<string, string> Links { get; set; }
        public Dictionary<string, object> Attributes { get; set; }
    }

    public class JsonApiRelation
    {

        public string Type { get; set; }
        public string Id { get; set; }
        public Dictionary<string, object> Attributes { get; set; }
    }
}