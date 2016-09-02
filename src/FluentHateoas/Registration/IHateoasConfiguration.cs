namespace FluentHateoas.Registration
{
    public interface IHateoasConfiguration
    {
        HrefStyle HrefStyle { get; set; }
        LinkStyle LinkStyle { get; set; }
        TemplateStyle TemplateStyle { get; set; }
    }
}