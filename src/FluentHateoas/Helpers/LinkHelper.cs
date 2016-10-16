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
                Template = p.Template?.Replace("{", ":").Replace("}", ""),
                Rel = p.Relation,
                p.Method,
                Command = p.Command?.Name,
            });
        }
    }
}