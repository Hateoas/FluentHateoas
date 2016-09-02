using System.Collections.Generic;
using System.Dynamic;

namespace FluentHateoas.Registration
{
    using System;

    public static class HateoasConfigurationExtensions
    {
        public static void Extend(this Interfaces.IHateoasConfiguration configuration, ExpandoObject vars)
        {
            var varsDictionary = (IDictionary<string, object>)vars;

            TryExtendHrefStyle(configuration, varsDictionary);
            TryExtendLinkStyle(configuration, varsDictionary);
            TryExtendTemplateStyle(configuration, varsDictionary);
        }

        private static void TryExtendHrefStyle(Interfaces.IHateoasConfiguration configuration, IDictionary<string, object> varsDictionary)
        {
            object hrefStyle;
            if (!varsDictionary.TryGetValue("HrefStyle", out hrefStyle))
            {
                return;
            }

            if (hrefStyle is HrefStyle)
            {
                configuration.HrefStyle = (HrefStyle)hrefStyle;
            }
            else if (hrefStyle is string)
            {
                HrefStyle hrefStyleEnum;
                if (Enum.TryParse(hrefStyle.ToString(), true, out hrefStyleEnum))
                {
                    configuration.HrefStyle = hrefStyleEnum;
                }
            }
        }

        private static void TryExtendLinkStyle(Interfaces.IHateoasConfiguration configuration, IDictionary<string, object> varsDictionary)
        {
            object linkStyle;
            if (!varsDictionary.TryGetValue("LinkStyle", out linkStyle))
            {
                return;
            }

            if (linkStyle is LinkStyle)
            {
                configuration.LinkStyle = (LinkStyle)linkStyle;
            }
            else if (linkStyle is string)
            {
                LinkStyle linkStyleEnum;
                if (Enum.TryParse(linkStyle.ToString(), true, out linkStyleEnum))
                {
                    configuration.LinkStyle = linkStyleEnum;
                }
            }
        }

        private static void TryExtendTemplateStyle(Interfaces.IHateoasConfiguration configuration, IDictionary<string, object> varsDictionary)
        {
            object templateStyle;
            if (!varsDictionary.TryGetValue("TemplateStyle", out templateStyle))
            {
                return;
            }

            if (templateStyle is TemplateStyle)
            {
                configuration.TemplateStyle = (TemplateStyle)templateStyle;
            }
            else if (templateStyle is string)
            {
                TemplateStyle templateStyleEnum;
                if (Enum.TryParse(templateStyle.ToString(), true, out templateStyleEnum))
                {
                    configuration.TemplateStyle = templateStyleEnum;
                }
            }
        }
    }
}