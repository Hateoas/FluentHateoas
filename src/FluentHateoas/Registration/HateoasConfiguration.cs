namespace FluentHateoas.Registration
{
    public class HateoasConfiguration
    {
        public HrefStyle HrefStyle { get; set; } = HrefStyle.Relative;
        public LinkStyle LinkStyle { get; set; } = LinkStyle.Array;
        public TemplateStyle TemplateStyle { get; set; } = TemplateStyle.Rendered;
    }
}