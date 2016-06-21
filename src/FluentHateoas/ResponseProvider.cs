using System.Net.Http;
using FluentHateoas.Builder;

namespace FluentHateoas
{
    public interface IResponseProvider
    {
        HttpResponseMessage Create(HttpResponseMessage response);
    }

    public class ResponseProvider : IResponseProvider
    {
        public ResponseProvider()
        { }

        public HttpResponseMessage Create(HttpResponseMessage response)
        {
            ObjectContent content;
            if (!response.StatusCode.IsSuccess() && !response.TryGetContent(out content))
                return response;

            // todo: Create ResponseMessage

            return response;
        }
    }
}