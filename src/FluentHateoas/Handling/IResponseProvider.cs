namespace FluentHateoas.Handling
{
    public interface IResponseProvider
    {
        System.Net.Http.HttpResponseMessage Create(System.Net.Http.HttpResponseMessage response);
    }
}