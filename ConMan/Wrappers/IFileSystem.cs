using System;
using System.IO;

namespace ConMan.Wrappers
{
    internal interface IFileSystem
    {
        string ReadAllText(string path);
        IDisposable MonitorFileChanges(string filePath, Action onChange);
        bool FileExists(string path);
    }
}