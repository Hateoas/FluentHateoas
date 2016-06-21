using System.Linq;
using System.Net;

namespace FluentHateoas
{
    internal static class StatusCodeExtensions
    {
        private static readonly HttpStatusCode[] SuccesCodes =
        {
            HttpStatusCode.OK, HttpStatusCode.Created, HttpStatusCode.Accepted, HttpStatusCode.NonAuthoritativeInformation,
            HttpStatusCode.NoContent, HttpStatusCode.ResetContent, HttpStatusCode.PartialContent
        };

        public static bool IsSuccess(this HttpStatusCode statusCode)
        {
            return SuccesCodes.Contains(statusCode);
        }
    }
}