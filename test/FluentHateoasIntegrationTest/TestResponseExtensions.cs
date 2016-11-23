using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;

namespace FluetHateoasIntegrationTest
{
    [ExcludeFromCodeCoverage]
    internal static class TestExtensions
    {
        public static void AddContent<TModel>(this HttpResponseMessage response, TModel data)
        {
            response.Content = new ObjectContent(
                data?.GetType() ?? typeof(TModel),
                data,
                new JsonMediaTypeFormatter(),
                "application/json");
        }

        public static async Task<string> GetAsString(this ObjectContent content)
        {
            string contentString;

            using (var stream = new MemoryStream())
            {
                var transportContext = default(TransportContext);
                var cancellationToken = CancellationToken.None;

                await content.Formatter.WriteToStreamAsync(
                    content.Value.GetType(),
                    content.Value,
                    stream,
                    content,
                    transportContext,
                    cancellationToken);

                stream.Position = 0;
                using (var sr = new StreamReader(stream))
                {
                    contentString = sr.ReadToEnd();
                }
            }

            return contentString;
        }
    }
}