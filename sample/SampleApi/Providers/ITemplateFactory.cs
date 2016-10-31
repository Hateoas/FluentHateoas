using System.Diagnostics.CodeAnalysis;
using SampleApi.Model;

namespace SampleApi.Providers
{
    public interface ITemplateFactory
    {
        object Create();
    }

    [ExcludeFromCodeCoverage]
    public class TemplateFactory : ITemplateFactory
    {
        public object Create()
        {
            return new CreateChildRequest();
        }
    }
}