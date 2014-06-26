using System;
using System.Collections.Generic;
using System.IO;
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

        public IList<string> GetDirectories()
        {
            return Directory.GetDirectories(_rootPath);
        }

        public DateTime GetDirectoryCreationTime(string bucketName)
        {
            return Directory.GetCreationTime(Path.Combine(_rootPath, bucketName));
        } 

        public Stream StreamOfFile(string fileName)
        {
            return File.Open(Path.Combine(_rootPath, fileName), FileMode.Open);
        }

        public string ReadToEnd(string fileName)
        {
            return File.ReadAllText(Path.Combine(_rootPath, fileName));
        }
    }
}
