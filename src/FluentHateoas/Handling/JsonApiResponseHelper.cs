using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.UI;
using FluentHateoas.Builder.Model;
using FluentHateoas.Helpers;
using FluentHateoas.Registration;

namespace FluentHateoas.Handling
{
    public static class JsonApiResponseHelper
    {
        public static HttpResponseMessage Ok(HttpRequestMessage request, object model, IEnumerable<IHateoasLink> links, NullValueHandling nullValueHandling)
        {
            return CreateResponse(
                request, 
                model.IsOrImplementsIEnumerable() ? CreateCollectionResponse(model as IEnumerable<object>, links, nullValueHandling) : CreateResponse(model, links, nullValueHandling), 
                System.Net.HttpStatusCode.OK);
        }

        private static JsonApiResponse CreateCollectionResponse<TModel>(IEnumerable<TModel> model, IEnumerable<IHateoasLink> links, NullValueHandling nullValueHandling)
        {
            var objectType = model.GetType().GenericTypeArguments[0];
            var collectionType = objectType.MakeIEnumerableOfType();

            var array = ((IEnumerable<object>)model).ToArray();
            var properties = objectType.GetProperties();

            var linksArray = links.ToArray();

            var memberNames = linksArray
                .Where(p => p.IsMember)
                .Select(p => new { origin = (p.MemberId.Origin ?? p.MemberId.Name).ToLowerInvariant(), relation = p.Relation.ToLowerInvariant() })
                .ToArray();

            var idProperty = GetIdProperty(objectType, properties);
            var includes = new List<JsonApiData>();
            var entities = array.Select(item =>
            {
                var entity = CreateRelation<JsonApiEntity>(item, objectType, idProperty, properties, nullValueHandling, memberNames.SelectMany(p => new[] { p.origin, p.relation }).ToArray());
                includes.AddRange(GetIncludes(item, nullValueHandling, memberNames.Select(p => p.relation).ToArray()));

                entity.Links = CreateLinks(linksArray);
                entity.Relationships = CreateRelationships(item, linksArray);

                return entity;
            }).ToList();

            return new CollectionResponse
            {
                Data = entities,
                Includes = includes.Where(p => p != null).DistinctBy(p => new { p.Id, p.Type }).ToList()
            };
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
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

        private static JsonApiResponse CreateResponse(object model, IEnumerable<IHateoasLink> links, NullValueHandling nullValueHandling)
        {
            var objectType = model.GetType();
            var properties = objectType.GetProperties();

            var linksArray = links.ToArray();

            var memberNames = linksArray
                .Where(p => p.IsMember)
                .Select(p => new { origin = (p.MemberId.Origin ?? p.MemberId.Name).ToLowerInvariant(), relation = p.Relation.ToLowerInvariant() })
                .ToArray();

            var idProperty = GetIdProperty(objectType, properties);
            var entity = CreateRelation<JsonApiEntity>(model, objectType, idProperty, properties, nullValueHandling, memberNames.SelectMany(p => new[] { p.origin, p.relation }).ToArray());
            var includes = GetIncludes(model, nullValueHandling, memberNames.Select(p => p.relation).ToArray());

            entity.Links = CreateLinks(linksArray);
            entity.Relationships = CreateRelationships(model, linksArray);

            return new SingleResponse
            {
                Data = entity,
                Includes = includes.Where(p => p != null).DistinctBy(p => new { p.Id, p.Type }).ToList()
            };
        }

        private static Dictionary<string, JsonApiRelationship> CreateRelationships(object model, IHateoasLink[] linksArray)
        {
            var result = new Dictionary<string, JsonApiRelationship>();
            var links = linksArray.Where(p => p.IsMember).ToArray();
            var properties = model.GetType().GetProperties();

            Array.ForEach(links, hateoasLink =>
            {
                var property = properties.Single(p => p.Name == hateoasLink.Relation);
                var propertyValue = property.GetValue(model);

                JsonApiRelationship relation;
                if (property.PropertyType.IsOrImplementsIEnumerable())
                {
                    relation = CreateRelations(propertyValue, property.PropertyType.GenericTypeArguments[0], hateoasLink);
                }
                else
                {
                    relation = CreateRelation(property.GetValue(model), GetIdProperty(property.PropertyType, property.PropertyType.GetProperties()), hateoasLink);
                }

                result.Add(hateoasLink.Relation, relation);
            });
            return result;
        }

        private static Dictionary<string, string> CreateLinks(IHateoasLink[] linksArray)
        {
            return linksArray
                .Where(p => !p.IsMember && string.IsNullOrWhiteSpace(p.Template))
                .ToDictionary(p => p.Relation, p => p.LinkPath ?? p.Template);
        }

        private static List<JsonApiData> GetIncludes(object model, NullValueHandling nullValueHandling, params string[] memberNames)
        {
            var properties = model
                .GetType()
                .GetProperties()
                .Where(p => memberNames.Any(m => m == p.Name.ToLowerInvariant()))
                .ToList();

            return properties.SelectMany(p =>
            {
                var propertyModel = p.GetValue(model);
                if (p.PropertyType.IsOrImplementsIEnumerable())
                {
                    var elementType = p.PropertyType.GenericTypeArguments[0];
                    var propertyProperties = elementType.GetProperties();
                    var idProperty = GetIdProperty(elementType, propertyProperties);
                    return ((IEnumerable<object>)propertyModel).Select(m => CreateRelation<JsonApiData>(m, elementType, idProperty, propertyProperties, nullValueHandling));
                }
                else
                {
                    var propertyProperties = p.PropertyType.GetProperties();
                    var idProperty = GetIdProperty(p.PropertyType, propertyProperties);
                    return new[] { CreateRelation<JsonApiData>(propertyModel, p.PropertyType, idProperty, propertyProperties, nullValueHandling) };
                }
            }).ToList();
        }

        private static SingleRelation CreateRelation(object model, PropertyInfo property, IHateoasLink hateoasLink)
        {
            if (model == null)
                return new SingleRelation { Data = new JsonApiEntity {Type = property.DeclaringType.Name } };

            var id = property.GetValue(model).ToString();

            return new SingleRelation
            {
                Links = new Dictionary<string, string> {{"self", hateoasLink.LinkPath ?? hateoasLink.Template.Replace("{id}", id) }},
                Data = new JsonApiData
                {
                    Type = model.GetType().Name,
                    Id = id
                }
            };
        }

        private static CollectionRelation CreateRelations(object model, Type propertyType, IHateoasLink hateoasLink)
        {
            var result = new CollectionRelation
            {
                Links = new Dictionary<string, string> { { "self", hateoasLink.LinkPath } },
            };

            if (model == null)
                return result;

            var collection = ((IEnumerable<object>)model).ToList();
            if (!collection.Any())
                return result;

            var idProperty = GetIdProperty(propertyType, propertyType.GetProperties());
            result.Data = collection.Select(p => new JsonApiData
            {
                Type = propertyType.Name,
                Id = GetValueFromModel(p, idProperty)
            }).ToList();

            return result;
        }

        private static TModel CreateRelation<TModel>(object model, Type objectType, PropertyInfo idProperty, PropertyInfo[] properties, NullValueHandling nullValueHandling, params string[] memberNames) where TModel : JsonApiData, new()
        {
            if (model == null)
                return null;

            var result = new TModel
            {
                Id = GetValueFromModel(model, idProperty),
                Type = objectType.Name,
            };

            var attributes = GetAttributes(model, memberNames, properties, idProperty, nullValueHandling);
            result.Attributes = attributes;

            return result;
        }

        private static string GetValueFromModel<TModel>(TModel model, PropertyInfo idProperty)
        {
            return model.GetType().GetProperty(idProperty.Name).GetValue(model).ToString();
        }

        private static Dictionary<string, object> GetAttributes(object model, IEnumerable<string> memberNames, IEnumerable<PropertyInfo> modelProperties, PropertyInfo idProperty, NullValueHandling nullValueHandling)
        {
            var result = model.GetType().GetProperties()
                .Where(p => p.Name != idProperty.Name)
                .Where(p => memberNames.All(m => m != p.Name.ToLowerInvariant()))
                .ToDictionary(p => p.Name, p => p.GetValue(model));

            return nullValueHandling == NullValueHandling.Ignore
                ? result.Where(p => p.Value != null).ToDictionary(p => p.Key, p => p.Value)
                : result;
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
                throw new Exception($"JsonApi: Unable to determine an ID-property. Please provide an property with the name \"Id\"/\"{objectType.Name}Id\" or adding a [Key]-attribute to a property");

            return keyProperties.First();

        }

        private static HttpResponseMessage CreateResponse(HttpRequestMessage request, JsonApiResponse response, HttpStatusCode statusCode)
        {
            return request.CreateResponse(statusCode, response);
        }
    }
}