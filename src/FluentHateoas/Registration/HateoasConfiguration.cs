namespace FluentHateoas.Registration
{
    public class HateoasConfiguration : Interfaces.IHateoasConfiguration
    {
        public HrefStyle HrefStyle { get; set; } = HrefStyle.Relative;
        public LinkStyle LinkStyle { get; set; } = LinkStyle.Array;
        public TemplateStyle TemplateStyle { get; set; } = TemplateStyle.Rendered;
        public ResponseStyle ResponseStyle { get; set; } = ResponseStyle.Hateoas;
        public NullValueHandling NullValueHandling { get; set; } = NullValueHandling.Include;
    }
}