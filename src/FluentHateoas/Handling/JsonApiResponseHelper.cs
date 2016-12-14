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
            return CreateResponse(request, model.IsOrImplementsIEnumerable() 
                ? CreateCollectionResponse(model as IEnumerable<object>, links) 
                : CreateResponse(model, links), System.Net.HttpStatusCode.OK);
        }

        private static JsonApiResponse CreateCollectionResponse<TModel>(IEnumerable<TModel> model, IEnumerable<IHateoasLink> links)
        {
            var objectType = model.GetType().GenericTypeArguments[0];
            var collectionType = objectType.MakeIEnumerableOfType();

            var array = ((IEnumerable<object>) model).ToArray();
            var properties = objectType.GetProperties();

            var linksArray = links.ToArray();

            var memberNames = linksArray
                .Where(p => p.IsMember)
                .Select(p => new { origin = p.MemberId.Origin.ToLowerInvariant(), relation = p.Relation.ToLowerInvariant() })
                .ToArray();

            var idProperty = GetIdProperty(objectType, properties);
            var includes = new List<JsonApiRelation>();
            var entities = array.Select(item =>
            {
                var entity = CreateRelation<JsonApiEntity>(item, objectType, idProperty, properties, memberNames.SelectMany(p => new[] {p.origin, p.relation}).ToArray());
                includes.AddRange(GetIncludes(item, memberNames.Select(p => p.relation).ToArray()));

                entity.Links = CreateLinks(linksArray);
                entity.Relationships = CreateRelationships(item, properties, linksArray);

                return entity;
            }).ToList();

            return new JsonApiResponse
            {
                Data = entities,
                Includes = includes.DistinctBy(p => new { p.Id, p.Type }).ToList()
            };
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey> (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        private static JsonApiResponse CreateResponse(object model, IEnumerable<IHateoasLink> links)
        {
            var objectType = model.GetType();
            var properties = objectType.GetProperties();

            var linksArray = links.ToArray();

            var memberNames = linksArray
                .Where(p => p.IsMember)
                .Select(p => new { origin = p.MemberId.Origin.ToLowerInvariant(), relation = p.Relation.ToLowerInvariant() })
                .ToArray();

            var idProperty = GetIdProperty(objectType, properties);
            var entity = CreateRelation<JsonApiEntity>(model, objectType, idProperty, properties, memberNames.SelectMany(p => new[] { p.origin, p.relation }).ToArray());
            var includes = GetIncludes(model, memberNames.Select(p => p.relation).ToArray());

            entity.Links = CreateLinks(linksArray);
            entity.Relationships = CreateRelationships(model, properties, linksArray);

            return new JsonApiResponse
            {
                Data = entity,
                Includes = includes.DistinctBy(p => new { p.Id, p.Type }).ToList()
            };
        }

        private static Dictionary<string, JsonApiRelation> CreateRelationships(object model, IEnumerable<PropertyInfo> properties, IHateoasLink[] linksArray)
        {
            var result = new Dictionary<string, JsonApiRelation>();

            var links = linksArray.Where(p => p.IsMember).ToArray();

            Array.ForEach(links, hateoasLink => result.Add(hateoasLink.Relation, CreateRelation(properties.Single(p => p.Name == hateoasLink.Relation), hateoasLink)));

            return result;
        }

        private static Dictionary<string, string> CreateLinks(IHateoasLink[] linksArray)
        {
            return linksArray
                .Where(p => !p.IsMember && string.IsNullOrWhiteSpace(p.Template) && (string.IsNullOrWhiteSpace(p.Method) || p.Method == "GET"))
                .ToDictionary(p => p.Relation, p => p.LinkPath ?? p.Template);
        }

        private static List<JsonApiRelation> GetIncludes(object model, params string[] memberNames)
        {
            var properties = model
                .GetType()
                .GetProperties()
                .Where(p => memberNames.Any(m => m == p.Name.ToLowerInvariant()))
                .ToList();

            return properties.Select(p =>
            {
                var propertyModel = p.GetValue(model);
                var propertyProperties = p.PropertyType.GetProperties();
                var idProperty = GetIdProperty(p.PropertyType, propertyProperties);

                return CreateRelation<JsonApiRelation>(propertyModel, p.PropertyType, idProperty, propertyProperties);
            }).ToList();
        }

        private static JsonApiRelation CreateRelation(PropertyInfo property, IHateoasLink hateoasLink)
        {
            return new JsonApiRelation
            {
                Type = property.PropertyType.Name,
                Id = hateoasLink.MemberId.Value.ToString()
            };
        }

        private static TModel CreateRelation<TModel>(object model, Type objectType, PropertyInfo idProperty, PropertyInfo[] properties, params string[] memberNames) where TModel : JsonApiRelation, new()
        {
            var result = CreateSimpleRelation<TModel>(model, objectType, idProperty, properties);

            var attributes = GetAttributes(model, memberNames, properties, idProperty);
            result.Attributes = attributes;

            return result;
        }
        private static TModel CreateSimpleRelation<TModel>(object model, Type objectType, PropertyInfo idProperty, PropertyInfo[] properties) where TModel : JsonApiSimpleRelation, new()
        {
            var result = new TModel
            {
                Id = GetValueFromModel(model, idProperty),
                Type = objectType.Name
            };

            return result;
        }

        private static string GetValueFromModel<TModel>(TModel model, PropertyInfo idProperty)
        {
            return model.GetType().GetProperty(idProperty.Name).GetValue(model).ToString();
        }

        private static Dictionary<string, object> GetAttributes(object model, IEnumerable<string> memberNames, IEnumerable<PropertyInfo> modelProperties, PropertyInfo idProperty)
        {
            return model.GetType().GetProperties()
                .Where(p => p.Name != idProperty.Name)
                .Where(p => memberNames.All(m => m != p.Name.ToLowerInvariant()))
                .ToDictionary(p => p.Name, p => p.GetValue(model));
        }

        private static PropertyInfo GetIdProperty(Type objectType, PropertyInfo[] properties)
        {
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

        private static HttpResponseMessage CreateResponse(HttpRequestMessage request, JsonApiResponse response, System.Net.HttpStatusCode statusCode)
        {
            return request.CreateResponse(statusCode, response);
        }
    }
}