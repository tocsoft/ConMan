using System;
using System.Collections.Generic;
using ConMan.Providers;

namespace ConMan
{
    public class SettingsManager : ISettingsManager
    {
        public static SettingsManager GlobalInstance { get; set; } = new SettingsManager();


        private List<ISettingsManagerProvider> providers = new List<ISettingsManagerProvider>();

        public void RegisterProvider(ISettingsManagerProvider provider)
        {
            providers.Add(provider);
        }

        public string GetSetting(string path)
        {
            foreach (var p in providers)
            {
                var value = p.GetValue(path);
                if (value != null)
                    return value;
            }
            return null;
        }

    }
}
