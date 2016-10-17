using SampleApi.Model;

namespace SampleApi.Providers
{
    public interface ITemplateFactory
    {
        object Create();
    }

    public class TemplateFactory : ITemplateFactory
    {
        public object Create()
        {
            return new CreateChildRequest();
        }
    }
}