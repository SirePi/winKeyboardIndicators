using Microsoft.Win32;

namespace KeyIndicator.Client
{
    static class WindowsStartupHelper
    {
        private const string PROGRAM_NAME = @"SnowyPeak.KeyIndicatorWpf";
        private const string REGISTRY_KEY = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";

        public static void AddApplicationToStartup()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(REGISTRY_KEY, true))
            {
                key.SetValue(PROGRAM_NAME, "\"" + typeof(App).Assembly.Location + "\"");
            }
        }

        public static void RemoveApplicationFromStartup()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(REGISTRY_KEY, true))
                key.DeleteValue(PROGRAM_NAME, false);
        }

        public static bool IsApplicationInStartup()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(REGISTRY_KEY, false))
                return key.GetValue(PROGRAM_NAME) != null;
        }
    }
}
