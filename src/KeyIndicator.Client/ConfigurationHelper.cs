using System.Configuration;

namespace KeyIndicator.Client
{
    static class Config
    {
        public const string SHOW_ALL_INDICATORS = "ShowAllIndicators";
    }

    static class ConfigurationHelper
    {
        public static void SetVariable(string name, string value)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings.Remove(name);
            config.AppSettings.Settings.Add(name, value);
            config.Save(ConfigurationSaveMode.Modified);
        }

        public static string ReadVariable(string name)
        {
            return ConfigurationManager.AppSettings[name];
        }
    }
}
