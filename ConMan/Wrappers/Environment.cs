using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConMan.Wrappers
{
    internal class Environment : IEnvironment
    {
        public static IEnvironment Default { get; set; } = new Environment();
        public string GetEnvironmentVariable(string variableName)
        {
            return System.Environment.GetEnvironmentVariable(variableName);
        }
    }
}
