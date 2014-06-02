using System.Collections.Generic;
using System.IO;

namespace Api.Filesystem
{
    public interface IFilesystemProvider
    {
        IList<string> ListRootDirectory(bool includeFiles = false, bool includeFolders = true);
        FileStream GetFileStream(string path);
    }
}
