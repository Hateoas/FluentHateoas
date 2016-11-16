using System.Security.Principal;
using System.Web;

namespace FluentHateoas
{
    public class HttpContextWrapper : IHttpContext
    {
        public IPrincipal User => HttpContext.Current.User;
    }
}