namespace FluentHateoas.Interfaces
{
    public interface IHateoasConfiguration
    {
        Registration.HrefStyle HrefStyle { get; set; }
        Registration.LinkStyle LinkStyle { get; set; }
        Registration.TemplateStyle TemplateStyle { get; set; }
        Registration.ResponseStyle ResponseStyle { get; set; }
    }
}