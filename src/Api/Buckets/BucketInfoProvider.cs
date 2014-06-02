using System;
using System.Collections.Generic;
using System.Linq;
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
            return _fsProvider.ListRootDirectory().Select(s => new BucketInfo(s, DateTime.Now)).ToList();
        }
    }
}
