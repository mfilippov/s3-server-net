using System.Collections.Generic;
using Api.Domain;

namespace Api.Buckets
{
    public interface IBucketInfoProvider
    {
        IList<BucketInfo> GetBucketList();
    }
}
