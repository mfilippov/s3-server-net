using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Api.Filesystem
{
    public class FilesystemProvider : IFilesystemProvider
    {
        private readonly string _rootPath;

        public FilesystemProvider(string rootPath)
        {
            _rootPath = rootPath;
        }

        public bool Exists(string path, bool checkFile = true, bool checkFolder = true)
        {
            var absolutePath = Path.Combine(_rootPath, path);
            if (checkFile && checkFolder) return File.Exists(absolutePath) || Directory.Exists(absolutePath);
            if (checkFile) return File.Exists(absolutePath);
            if (checkFolder) return File.Exists(absolutePath);
            return false;
        }

        public IList<string> ListRootDirectory(bool includeFiles = false, bool includeFolders = true)
        {
            if (includeFiles && includeFolders) return Directory.GetFileSystemEntries(_rootPath).ToList();
            if (includeFolders) return Directory.GetDirectories(_rootPath).ToList();
            if (includeFiles) return Directory.GetFiles(_rootPath).ToList();
            return new List<string>();
        }

        public FileStream GetFileStream(string path)
        {
            return File.Open(Path.Combine(_rootPath, path), FileMode.Open);
        }
    }
}
