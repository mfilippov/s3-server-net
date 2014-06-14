using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Api.Domain;
using Api.Filesystem;

namespace Api.Buckets
{
    public class BucketInfoProvider : IBucketInfoProvider
    {
        private readonly IFilesystemProvider _fsProvider;

        public BucketInfoProvider(IFilesystemProvider fsProvider)
        {
            _fsProvider = fsProvider;
        }

        public IList<BucketInfo> GetBucketList()
        {
            return _fsProvider.GetBucketList().Select(bucketName => new BucketInfo {Name = bucketName, CreationDate = _fsProvider.GetBucketCreationDateTime(bucketName)}).ToList();
        }
    }
}
