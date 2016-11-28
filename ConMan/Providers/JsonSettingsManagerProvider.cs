using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConMan.Wrappers;
using Newtonsoft.Json.Linq;

namespace ConMan.Providers
{
    public class JsonSettingsManagerProvider : ISettingsManagerProvider, IDisposable
    {
        private readonly string filePath;
        private Dictionary<string, string> settings = null;
        private bool settingsLoaded = false;
        object _locker = new object();
        private IDisposable fsw;
        private readonly IFileSystem fileSystem;

        public JsonSettingsManagerProvider(string path)
            : this(path, FileSystem.Default)
        {
        }

        internal JsonSettingsManagerProvider(string path, IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
            this.filePath = path;
            FileExists = fileSystem.FileExists(path);

            fsw = fileSystem.MonitorFileChanges(filePath, () =>
            {
                //if file changed in any way we drop the settings object
                settings = null;
                settingsLoaded = false;
            });
        }

        public bool FileExists { get; }

        public string GetValue(string path)
        {
            if (EnsureSettingsLoaded())
            {
                return null;
            }

            if (settings.ContainsKey(path))
            {
                return settings[path];
            }

            return null;
        }

        private bool EnsureSettingsLoaded()
        {
            if (!settingsLoaded)
            {
                lock (_locker)
                {
                    //if null then try to load it from file
                    try
                    {
                        var localsettings = JObject.Parse(fileSystem.ReadAllText(filePath));
                        settings = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                        var all = localsettings.DescendantsAndSelf();
                        foreach (var s in all)
                        {
                            var v = s as JValue;
                            if (v != null)
                            {
                                settings.Add(v.Path, v.Value.ToString());
                            }
                        }
                    }
                    catch
                    {
                        settings = null;
                        //failed to load then act as empty
                    }
                    settingsLoaded = true;
                }
            }

            return (settings == null);
        }

        public void Dispose()
        {
            fsw?.Dispose();
        }
    }
}
