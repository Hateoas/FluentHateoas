using System.Reflection;
using FluentHateoas.Handling;
using FluentHateoas.Interfaces;

namespace FluentHateoas.Builder.Handlers
{
    public abstract class ArgumentProcessor : IArgumentProcessor
    {
        public abstract bool CanProcess<TModel>(IHateoasRegistration<TModel> registration, ILinkBuilder linkBuilder);
        public abstract bool Process<TModel>(IHateoasRegistration<TModel> registration, ILinkBuilder linkBuilder, object data);

        protected static string GetKey<TModel>(TModel data, MemberInfo member, bool isCollection = false)
        {
            return member.Name.Substring(0, 1).ToLowerInvariant() + member.Name.Substring(1);

            //var keyName = member.Name;
            //var valueType = isCollection ? data.GetType().GetGenericArguments()[0] : data.GetType();
            //var key = keyName == "Id"
            //    ? valueType.Name.Substring(0, 1).ToLowerInvariant() + valueType.Name.Substring(1) + keyName // PersonInfo.Id becomes personInfoId
            //    : keyName.Substring(0, 1).ToLowerInvariant() + keyName.Substring(1);
            //return key;
        }
    }
}