using System.Collections.Generic;
using System.Collections.Specialized;

namespace FluentHateoas.Handling
{
    public class JsonApiResponse : JsonApiEntity
    {
        internal JsonApiResponse() { }

        public NameValueCollection Relationships { get; set; }
    }

    public class JsonApiEntity
    {
        public string Type { get; set; }
        public string Id { get; set; }
        public NameValueCollection Attributes { get; set; }
    }
}