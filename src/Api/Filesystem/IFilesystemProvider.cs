using System.Collections.Generic;
using System.IO;

namespace Api.Filesystem
{
    public interface IFilesystemProvider
    {
        bool Exists(string path);
        IList<string> ListRootDirectory(bool includeFiles = false, bool includeFolders = true);
        Stream StreamOfFile(string path);
    }
}
