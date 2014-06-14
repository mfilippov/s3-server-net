using Api.Domain;

namespace Api.Security
{
    public interface IBucketLordTemple
    {
        BucketLord CallBucketLord(string name);
        BucketLord FindLordByAccessKeyId(string accessKeyId);
    }
}
