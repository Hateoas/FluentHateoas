using System.Net.Http;

namespace FluentHateoas.Handling
{
    public static class LinkBuilderExtensions
    {
        public static IHateoasLink Build(this LinkBuilder source)
        {
            var result = new HateoasLink
            {
                Rel = source.Relation
            };

            if (source.IsTemplate)
                result.Template = "generated template!";
            else
                result.Href = "generated href!";

            if (source.Method != HttpMethod.Get)
                result.Method = source.Method.ToString();

            return result;
        }
    }
}