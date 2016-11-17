using System.Security.Principal;
using System.Web;

namespace FluentHateoas
{
    public class HttpContextWrapper : IHttpContext
    {
        private readonly HttpContext _context;

        public HttpContextWrapper(HttpContext context)
        {
            _context = context;
        }

        public IPrincipal User => _context.User;
    }
}