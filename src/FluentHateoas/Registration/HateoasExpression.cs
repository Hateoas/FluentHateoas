using System;
using System.Linq.Expressions;
using System.Net.Http;
using FluentHateoas.Contracts;

namespace FluentHateoas.Registration
{
    public interface IHateoasExpression
    {
        Type Controller { get; set; }
        string Relation { get; set; }
        LambdaExpression TargetAction { get; set; }
        HttpMethod HttpMethod { get; set; }
    }

    public class HateoasExpression<TModel> : IHateoasExpression
    {
        private readonly HateoasRegistration<TModel> _registration;

        public HateoasExpression(HateoasRegistration<TModel> registration)
        {
            _registration = registration;
        }

        public Type Controller { get; set; }
        public string Relation { get; set; }
        public LambdaExpression TargetAction { get; set; }
        public HttpMethod HttpMethod { get; set; }
    }
}