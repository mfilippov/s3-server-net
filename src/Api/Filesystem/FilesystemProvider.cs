using System.Collections.Generic;
using System.IO;
using System.Linq;
using Api.Configuration;

namespace Api.Filesystem
{
    public class FilesystemProvider : IFilesystemProvider
    {
        private readonly string _rootPath;

        public FilesystemProvider(INodeConfiguration configuration)
        {
            _rootPath = configuration.RootPath;
        }

        public bool Exists(string path)
        {
            var absolutePath = Path.Combine(_rootPath, path);
            return File.Exists(absolutePath) || Directory.Exists(absolutePath);
        }

        public IList<string> ListRootDirectory(bool includeFiles = false, bool includeFolders = true)
        {
            if (includeFiles && includeFolders) return Directory.GetFileSystemEntries(_rootPath).ToList();
            if (includeFolders) return Directory.GetDirectories(_rootPath).ToList();
            if (includeFiles) return Directory.GetFiles(_rootPath).ToList();
            return new List<string>();
        }

        public Stream StreamOfFile(string path)
        {
            return File.Open(Path.Combine(_rootPath, path), FileMode.Open);
        }
    }
}
