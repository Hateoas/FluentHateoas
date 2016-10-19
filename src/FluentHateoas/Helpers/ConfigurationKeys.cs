using System.Configuration;

namespace FluentHateoas.Helpers
{
    internal static class ConfigurationKeys
    {
        public static string ApiPrefix => $"/{(GetSetting("api-prefix") ?? "api")}/";

        private static string GetSetting(string key)
        {
            var format = $"fluenthateoas:{key}";
            return ConfigurationManager.AppSettings[format];
        }
    }
}