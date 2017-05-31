using System.Collections.Generic;

namespace Fluenthateoas.JsonApi.Handlers
{
    public abstract class JsonApiResponse
    {
        public List<JsonApiData> Includes { get; set; }
    }

    public class CollectionResponse : JsonApiResponse
    {
        public ICollection<JsonApiEntity> Data { get; set; }
    }

    public class SingleResponse : JsonApiResponse
    {
        public JsonApiEntity Data { get; set; }
    }

    public class JsonApiEntity : JsonApiData
    {
        public Dictionary<string, JsonApiRelationship> Relationships { get; set; }
        public Dictionary<string, string> Links { get; set; }
    }

    public abstract class JsonApiRelationship
    {
        public Dictionary<string, string> Links { get; set; }
    }

    public class SingleRelation : JsonApiRelationship
    {
        public JsonApiData Data { get; set; }
    }
    public class CollectionRelation : JsonApiRelationship
    {
        public ICollection<JsonApiData> Data { get; set; }
    }

    public class JsonApiData
    {
        public string Type { get; set; }
        public string Id { get; set; }
        public Dictionary<string, object> Attributes { get; set; }
    }
}