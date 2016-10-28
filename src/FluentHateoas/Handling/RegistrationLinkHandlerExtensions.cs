namespace FluentHateoas.Handling
{
    public static class RegistrationLinkHandlerExtensions
    {
        public static IRegistrationLinkHandler CreateChain(this System.Collections.Generic.IEnumerable<IRegistrationLinkHandler> source)
        {
            IRegistrationLinkHandler root = null;
            IRegistrationLinkHandler predecessor = null;

            foreach (var handler in source)
            {
                // set root
                if (root == null) root = handler;

                // set successor (if predecessor)
                predecessor?.SetSuccessor(handler);

                // set next predecessor
                predecessor = handler;
            }

            return root;
        }
    }
}