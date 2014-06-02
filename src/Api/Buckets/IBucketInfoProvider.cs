using System.Collections.Generic;

namespace Api.Buckets
{
    interface IBucketInfoProvider
    {
        IList<BucketInfo> GetBucketList();
    }
}
