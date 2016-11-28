using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConMan.Providers
{
    public class AppSettingsSettingsManagerProvider : ISettingsManagerProvider
    {
        public string GetValue(string path)
        {
            //app settings are seperated by colon first and fallback to seperated by dots
            return ConfigurationManager.AppSettings[path.Replace('.', ':')] ?? ConfigurationManager.AppSettings[path];
        }
    }
}
