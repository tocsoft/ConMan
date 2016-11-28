using System;
using System.IO;

namespace ConMan.Wrappers
{
    internal interface IEnvironment
    {
        string GetEnvironmentVariable(string variableName);
    }
}