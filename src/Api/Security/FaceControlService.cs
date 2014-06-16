using Api.Domain;
using Nancy;

namespace Api.Security
{
    public class FaceControlService : IFaceControlService
    {
        private readonly IBucketLordTemple _lordTemple;

        public FaceControlService(IBucketLordTemple lordTemple)
        {
            _lordTemple = lordTemple;
        }

        public BucketLord CheckAuth(Request req)
        {
            var s3Req = req.ToS3Request();

            if (s3Req != null)
            {
                var bucketLord = _lordTemple.FindLordByAccessKeyId(s3Req.S3Authorization.Credentials.AccessKeyId);
                if (bucketLord == null) return null;

                if (s3Req.Validate(bucketLord)) return bucketLord;
            }

            return null;
        }
    }
}
