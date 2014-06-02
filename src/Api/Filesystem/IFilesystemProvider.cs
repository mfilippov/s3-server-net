using System.Collections.Generic;
using System.IO;

namespace Api.Filesystem
{
    public interface IFilesystemProvider
    {
        bool Exists(string path, bool checkFile = true, bool checkFolder = true);
        IList<string> ListRootDirectory(bool includeFiles = false, bool includeFolders = true);
        FileStream GetFileStream(string path);
    }
}
