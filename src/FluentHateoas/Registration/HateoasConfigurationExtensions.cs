using System.CodeDom;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;

namespace FluentHateoas.Registration
{
    using System;

    public static class HateoasConfigurationExtensions
    {
        public static void Extend(this Interfaces.IHateoasConfiguration configuration, ExpandoObject vars)
        {
            var varsDictionary = (IDictionary<string, object>)vars;

            TryExtend(configuration, varsDictionary, c => c.HrefStyle);
            TryExtend(configuration, varsDictionary, c => c.LinkStyle);
            TryExtend(configuration, varsDictionary, c => c.TemplateStyle);
            TryExtend(configuration, varsDictionary, c => c.ResponseStyle);
        }

        private static void TryExtend<T>(Interfaces.IHateoasConfiguration configuration, IDictionary<string, object> varsDictionary, Expression<Func<Interfaces.IHateoasConfiguration, T>> expression) where T : struct
        {
            var property = ((MemberExpression) expression.Body).Member.Name;

            object value;
            if (!varsDictionary.TryGetValue(property, out value))
            {
                return;
            }

            if (value is T)
            {
                configuration.GetType().GetProperties().Single(p => p.Name == property).SetValue(configuration, value);
            }
            else if (value is string)
            {
                T templateStyleEnum;
                if (Enum.TryParse(value.ToString(), true, out templateStyleEnum))
                {
                    configuration.GetType().GetProperties().Single(p => p.Name == property).SetValue(configuration, templateStyleEnum);
                }
            }
        }
    }
}