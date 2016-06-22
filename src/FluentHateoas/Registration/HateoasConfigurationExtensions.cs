using System.Collections.Generic;
using System.Dynamic;

namespace FluentHateoas.Registration
{
    public static class HateoasConfigurationExtensions
    {
        public static void Extend(this HateoasConfiguration configuration, ExpandoObject vars)
        {
            var varsDictionary = (IDictionary<string, object>)vars;

            object hrefStyle;
            if (varsDictionary.TryGetValue("HrefStyle", out hrefStyle) && hrefStyle is HrefStyle)
                configuration.HrefStyle = (HrefStyle)hrefStyle;

            object linkStyle;
            if (varsDictionary.TryGetValue("LinkStyle", out linkStyle) && linkStyle is LinkStyle)
                configuration.LinkStyle = (LinkStyle)linkStyle;

            object templateStyle;
            if (varsDictionary.TryGetValue("TemplateStyle", out templateStyle) && templateStyle is TemplateStyle)
                configuration.TemplateStyle = (TemplateStyle)templateStyle;
        }
    }
}