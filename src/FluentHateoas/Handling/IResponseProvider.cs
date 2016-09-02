using System.Net.Http;

namespace FluentHateoas.Handling
{
    public interface IResponseProvider
    {
        HttpResponseMessage Create(HttpRequestMessage request, HttpResponseMessage response);
    }
}