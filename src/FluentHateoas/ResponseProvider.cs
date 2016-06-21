using System.Net.Http;

namespace FluentHateoas
{
    public interface IResponseProvider
    {
        HttpResponseMessage Create(HttpResponseMessage response);
    }

    public class ResponseProvider : IResponseProvider
    {
        public HttpResponseMessage Create(HttpResponseMessage response)
        {
            if (!response.StatusCode.IsSuccess())
                return response;

            return response;
        }
    }
}