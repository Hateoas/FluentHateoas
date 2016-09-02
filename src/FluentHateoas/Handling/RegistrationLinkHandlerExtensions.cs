using Enumerable = System.Linq.Enumerable;

namespace FluentHateoas.Handling
{
    public static class RegistrationLinkHandlerExtensions
    {
        public static IRegistrationLinkHandler CreateChain(this System.Collections.Generic.IEnumerable<IRegistrationLinkHandler> source)
        {
            var handlers = Enumerable.ToList(source);

            if (handlers.Count <= 1)
                return Enumerable.First(handlers);

            for (var i = 1; i < handlers.Count; i++)
            {
                handlers[i - 1].SetSuccessor(handlers[i]);
            }
            return Enumerable.First(handlers);
        }
    }
}