using System.Collections.Generic;

namespace Api.Filesystem
{
    public interface IFilesystemProvider
    {
        IList<string> ListRootDirectory(bool includeFiles = false, bool includeFolders = true);
    }
}
