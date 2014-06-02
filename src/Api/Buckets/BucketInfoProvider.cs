using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
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
            var bucketInfoSerializer = new XmlSerializer(typeof(BucketInfo));

            return _fsProvider.ListRootDirectory()
                .Where(s => _fsProvider.Exists(Path.Combine(s, "metadata.xml")))
                .Select(s => bucketInfoSerializer.Deserialize(_fsProvider.GetFileStream(Path.Combine(s, "metadata.xml"))) as BucketInfo).ToList();
        }
    }
}
