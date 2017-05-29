using System.Web;

namespace FluentHateoas.Handling
{
    public interface IMessageSerializer
    {
        bool HandlesContentType(string contentType);

        object OnRequest(System.Net.Http.HttpRequestMessage request, System.Threading.CancellationToken cancellationToken);
        object OnResponse(System.Net.Http.HttpRequestMessage request, System.Net.Http.HttpResponseMessage response);
    }
}