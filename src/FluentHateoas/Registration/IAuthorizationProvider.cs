using System.Reflection;

namespace FluentHateoas.Registration
{
    public interface IAuthorizationProvider
    {
        bool IsAuthorized(MethodInfo methodInfo);
    }
}