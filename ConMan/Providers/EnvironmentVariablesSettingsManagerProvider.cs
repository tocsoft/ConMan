using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConMan.Wrappers;

namespace ConMan.Providers
{
    public class EnvironmentVariablesSettingsManagerProvider : ISettingsManagerProvider
    {
        private readonly IEnvironment env;

        public EnvironmentVariablesSettingsManagerProvider() : this(ConMan.Wrappers.Environment.Default) { }

        internal EnvironmentVariablesSettingsManagerProvider(IEnvironment env)
        {
            this.env = env;
        }
        public string GetValue(string path)
        {
            var envName = path.Replace('.', '_').ToUpper();
            return env.GetEnvironmentVariable(envName);
        }
    }
}
