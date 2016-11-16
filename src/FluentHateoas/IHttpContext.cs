using System.Security.Principal;

namespace FluentHateoas
{
    public interface IHttpContext
    {
        IPrincipal User { get; }
    }
}