using System.Net.Http;

namespace FluentHateoas
{
    public static class ResponseMessageExtensions
    {
        public static bool TryGetContent(this HttpResponseMessage response, out ObjectContent content)
        {
            content = response.GetContent();
            return content == null;
        }

        public static ObjectContent GetContent(this HttpResponseMessage response)
        {
            return response.Content as ObjectContent;
        }
    }
}