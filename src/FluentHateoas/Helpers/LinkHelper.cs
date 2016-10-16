using System.Collections.Generic;
using System.Linq;
using FluentHateoas.Handling;

namespace FluentHateoas.Helpers
{
    public static class LinkHelper
    {
        public static IEnumerable<object> ToLinkList(this IEnumerable<IHateoasLink> source)
        {
            return source.Select(p => new
            {
                Href = p.LinkPath,
                p.Template,
                Rel = p.Relation,
                p.Method,
                Command = p.Command == null ? null : p.Command.Name,
            });
        }
    }
}