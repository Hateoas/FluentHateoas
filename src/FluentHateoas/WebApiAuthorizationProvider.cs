using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web.Http;
using FluentHateoas.Registration;

namespace FluentHateoas
{
    public class WebApiAuthorizationProvider : IAuthorizationProvider
    {
        private readonly IHttpContext _httpContext;
        
        public WebApiAuthorizationProvider(IHttpContext httpContext)
        {
            _httpContext = httpContext;
        }
        public string[] GetRoles(AuthorizeAttribute authorizeAttribute)
        {
            return SplitString(authorizeAttribute.Roles);
        }

        public string[] GetUsers(AuthorizeAttribute authorizeAttribute)
        {
            return SplitString(authorizeAttribute.Users);
        }

        public bool IsAuthorized(MethodInfo methodInfo)
        {
            var authorizeAttribute = methodInfo.DeclaringType?.GetCustomAttribute<AuthorizeAttribute>() ?? methodInfo.GetCustomAttribute<AuthorizeAttribute>();

            if (authorizeAttribute == null)
                return true;

            var user = _httpContext.User ?? Thread.CurrentPrincipal;
            if (user == null || user.Identity == null || !user.Identity.IsAuthenticated)
            {
                return false;
            }

            var users = GetUsers(authorizeAttribute);
            if (users.Length > 0 && !users.Contains(user.Identity.Name, StringComparer.OrdinalIgnoreCase))
            {
                return false;
            }

            var roles = GetRoles(authorizeAttribute);
            if (roles.Length > 0 && !roles.Any(user.IsInRole))
            {
                return false;
            }

            return true;
        }

        internal static string[] SplitString(string original)
        {
            if (String.IsNullOrEmpty(original))
            {
                return new string[0];
            }

            var split = from piece in original.Split(',')
                let trimmed = piece.Trim()
                where !String.IsNullOrEmpty(trimmed)
                select trimmed;
            return split.ToArray();
        }
    }
}