using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConMan.Providers;

namespace ConMan
{
    public static class SettingsManagerProviderRegistras
    {
        public static SettingsManager AddFromConfig(this SettingsManager settings)
        {
            settings.RegisterProvider(new AppSettingsSettingsManagerProvider());
            return settings;
        }

        public static SettingsManager AddEnvironmentVariables(this SettingsManager settings)
        {
            settings.RegisterProvider(new EnvironmentVariablesSettingsManagerProvider());
            return settings;
        }

        public static SettingsManager AddDevelopmentSettings(this SettingsManager settings, string name)
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var path = Path.Combine(appData, "Development", "Settings", name + ".json");
            
            var provider = new JsonSettingsManagerProvider(path);
            if (provider.FileExists)
            {
                //only allow registration if file exists on load to prevent overhead in production
                settings.RegisterProvider(provider);
            }else
            {
                provider.Dispose();
            }
            return settings;
        }
    }
}
