using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Web.UI;
using FluentHateoas.Helpers;

namespace FluentHateoas.Handling
{
    public static class JsonApiResponseHelper
    {
        public static HttpResponseMessage Ok(HttpRequestMessage request, object model, IEnumerable<IHateoasLink> links)
        {
            return CreateResponse(request, CreateHateoasResponse(model, links), System.Net.HttpStatusCode.OK);
        }

        private static JsonApiResponse CreateHateoasResponse(object model, IEnumerable<IHateoasLink> links)
        {
            var idProperty = model.GetIdProperty();

            return new JsonApiResponse
            {
                Data = new JsonApiEntity
                {
                    Type = model.GetType().Name,
                    Id = idProperty.GetValue(model).ToString(),
                    Attributes = model.AsJsonApiAttributes(idProperty),
                    Links = links.AsJsonApiLinks()
                }
            };
        }

        private static HttpResponseMessage CreateResponse(HttpRequestMessage request, JsonApiResponse response, System.Net.HttpStatusCode statusCode)
        {
            return request.CreateResponse(statusCode, response);
        }
    }

    internal static class JsonApiExtensions
    {
        internal static Dictionary<string, JsonApiRelation> AsJsonApiRelationships(this IEnumerable<IHateoasLink> source)
        {
            var result = new Dictionary<string, JsonApiRelation>();

            //Array.ForEach(source.ToArray(), p =>
            //{
            //    result.Add(p.Relation, );
            //});

            return result;
        }

        internal static Dictionary<string, string> AsJsonApiLinks(this IEnumerable<IHateoasLink> source)
        {
            var result = new Dictionary<string, string>();

            Array.ForEach(source.ToArray(), p =>
            {
                result.Add(p.Relation, p.LinkPath ?? p.Template);
            });

            return result;
        }

        internal static PropertyInfo GetIdProperty(this object obj)
        {
            var objectType = obj.GetType();
            var properties = objectType.GetProperties();

            var idProperty = properties.SingleOrDefault(p => p.Name == "Id");
            if (idProperty != null)
                return idProperty;

            idProperty = properties.SingleOrDefault(p => p.Name == objectType.Name + "Id");
            if (idProperty != null)
                return idProperty;

            var keyProperties = properties.Where(p => p.GetCustomAttribute<KeyAttribute>() != null).ToList();

            if (!keyProperties.Any() || keyProperties.Count() > 1)
                throw new Exception("JsonApiResponse: Unable to determine id");

            return keyProperties.First();
        }

        internal static Dictionary<string, object> AsJsonApiAttributes(this object obj, params PropertyInfo[] skipMembers)
        {
            var result = new Dictionary<string, object>();
            var properties = obj
                .GetType()
                .GetProperties()
                .Where(p => skipMembers.All(s => s.Name != p.Name))
                .ToList();

            properties.ForEach(p => result.Add(p.Name, p.GetValue(obj, null)));

            return result;
        }
    }
}