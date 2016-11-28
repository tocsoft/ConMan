using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConMan.Wrappers
{
    internal class FileSystem : IFileSystem
    {
        private readonly EmptyDisposable EmptyIDisposable = new FileSystem.EmptyDisposable();
        public static IFileSystem Default { get; set; } = new FileSystem();

        public bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        public bool FileExists(string path)
        {
            return File.Exists(path);
        }

        public string GetDirectoryName(string path)
        {
            return Path.GetDirectoryName(path);
        }

        public string GetFullPath(string path)
        {
            return Path.GetFullPath(path);
        }

        public IDisposable MonitorFileChanges(string filePath, Action onChange)
        {
            filePath = GetFullPath(filePath);

            if (onChange == null)
            {
                return EmptyIDisposable;
            }

            string directory = GetDirectoryName(filePath);
            if (!DirectoryExists(directory))
            {
                return EmptyIDisposable;
            }

            var fsw = new FileSystemWatcher(directory, "*.json");
            Action<FileSystemEventArgs> change = (e) =>
            {
                if (e.FullPath == filePath)
                {
                    onChange();
                }
            };

            FileSystemEventHandler realCallback = delegate (object s, FileSystemEventArgs e)
            {
                change(e);
            };

            fsw.Changed += realCallback;
            fsw.Created += realCallback;
            fsw.Deleted += realCallback;
            fsw.Renamed += delegate (object s, RenamedEventArgs e)
            {
                change(e);
            };

            fsw.EnableRaisingEvents = true;

            return fsw;
        }

        public string ReadAllText(string path)
        {
            path = GetFullPath(path);
            return File.ReadAllText(path);
        }

        private class EmptyDisposable : IDisposable
        {
            public void Dispose()
            {
                throw new NotImplementedException();
            }
        }
    }
}
